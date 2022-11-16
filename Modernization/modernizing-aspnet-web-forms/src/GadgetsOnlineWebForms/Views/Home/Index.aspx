﻿<%@ Page Title="Welcome to Gadgets Online!" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="GadgetsOnlineWebForms.Views.Home.Index" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div id="promotion">

</div>

<h3><em>Our best selling </em>Gadgets Online.</h3>

<class ="row">


   <ul id="album-list">
        <% foreach (var product in Model)
            {
                var productUrl = "/Views/Store/Details.aspx?productId=" + product.ProductId.ToString();
                %>
            <li>
                <a href="<%= productUrl %>" >

                    <img alt="<%= product.Name %>" src="<%= product.ProductArtUrl %>" />
                    <span><%= @product.Name %></span>
                </a>
            </li>
        <% } %>
    </ul>



</asp:Content>