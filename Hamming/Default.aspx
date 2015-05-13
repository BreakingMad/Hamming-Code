<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Hamming._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Код Хэмминга</h1>
        <p class="lead"></p>
        <p><asp:Button ID="Button1" runat="server" Text="Закодировать сообщение" class="btn btn-primary btn-large" OnClick="Button1_Click" />
        </p>
    </div>

    <div class="row">
        <div class="col-md-4">
          <div class="thumbnail" style="height: 111px;">
           <h4>Сообщение</h4>
           <p><input ID="TextBox1" runat="server" placeholder="Сообщение" value="1001" onkeypress='validate(event)'></input></p>
          </div>
        </div>
        <div class="col-md-4">
            <div class="thumbnail" style="height: 111px;">
                <p><asp:Label ID="Label1" runat="server"></asp:Label></p> 
                <p><asp:Label ID="Label2" runat="server"></asp:Label></p> 
                <p><asp:Label ID="Label3" runat="server"></asp:Label></p> 
            </div>
        </div>
        <div class="col-md-4">
           <div class="thumbnail" style="height: 111px;">
              <h4>Введите позиции ошибок через пробел</h4>
              <p><asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></p>
              <p><asp:Button ID="Button2" runat="server" CssClass="btn btn-primary" Text="Ввести" OnClick="Button2_Click" /></p>
           </div>
        </div>
    </div>
    
    
    <div class="row"  style="margin-left: 300px; margin-top: 10px;">
     <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
    </div>

<script type="text/javascript">
    function validate(evt) {
        var theEvent = evt || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
        var regex = /[0-1]|\./;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    }
</script>

</asp:Content>

