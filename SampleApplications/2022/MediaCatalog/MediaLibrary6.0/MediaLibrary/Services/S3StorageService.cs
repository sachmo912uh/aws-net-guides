﻿using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Options;

namespace MediaLibrary.Services
{
    public class S3StorageService : IStorageService
    {
        private readonly AwsSettings _configuration;
        private readonly string _baseFileLocation;
        private readonly ILogger<S3StorageService> _logger;

        private IAmazonS3 _s3 = new AmazonS3Client(RegionEndpoint.USEast2);


        public S3StorageService(IOptions<AwsSettings> options, ILogger<S3StorageService> logger)
        {
            _configuration = options.Value;
            _baseFileLocation = _configuration.BucketName;
            _logger = logger;

        }
        /// <summary>
        /// Write the file to primary storage, retruing the file storage location.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string SaveFile(IFormFile file)
        {
            TransferUtility utility = new TransferUtility(_s3);
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
            string objectName = string.Format("{0}{1}", new[] { Guid.NewGuid().ToString(), Path.GetExtension(file.FileName) });

            //_logger.Log(LogLevel.Information, "", new object[] {  });

            request.BucketName = _baseFileLocation;
            request.Key = objectName;
            try
            {
                var stream = new MemoryStream();
                file.CopyTo(stream);

                request.InputStream = file.OpenReadStream();
                utility.Upload(request);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
            }
            finally
            {
                request.InputStream.Close();
            }
            return objectName;
        }

        public Task<string[]> GetFileList()
        {
            return new Task<string[]> (() =>
                {
                    return new string[0];
                });

        }

        public Task<int> PurgeFiles()
        {
            return new Task<int>(() =>
                {
                    return 0;
                });
        }

        public async Task<bool> DeleteFile(string KeyName)
        {
            try
            {
                await _s3.DeleteAsync(_baseFileLocation, KeyName, null);
                return true;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return false;
            }
        }
    }
}
