﻿<%@Page Language="C#"%>

<script runat="server">
	void Page_Load(object sender, EventArgs e)
	{
		Response.Redirect("~/Test.aspx?testname=TestHF");
	}
</script>