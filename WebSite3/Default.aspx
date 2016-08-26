<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script type="text/x-mathjax-config">
        MathJax.Hub.Config({tex2jax: {inlineMath: [['$','$'], ['\\(','\\)']]}});
    </script>
    <script type="text/javascript" async
            src="https://cdn.mathjax.org/mathjax/latest/MathJax.js?config=TeX-AMS_CHTML">
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="TextBox1" runat="server" Width="659px"></asp:TextBox>        
            <asp:Button ID="Button1" runat="server" Text="Решить" OnClick="Button1_Click" Width="238px" />
        </div>
    <div id="result" runat="server">
    </div>
    </form>
    <div id="history" runat="server">
        -------------------------------------------------------------------------------------- $$ $$
        История решения математических выражений подобно этому \(\frac{1}{3}\) или этому:
        $$x = {-b \pm \sqrt{b^2-4ac} \over 2a}.$$
    </div>
    <div style="left: auto; bottom: auto; right: auto; top: auto">
        hjhj
    </div>
</body>
</html>
