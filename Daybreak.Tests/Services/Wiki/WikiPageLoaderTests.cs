using Daybreak.Services.Wiki;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using System.Extensions.Core;

namespace Daybreak.Tests.Services.Wiki;

[TestClass]
public sealed class WikiPageLoaderTests
{
    [TestMethod]
    public void PrepareDocument_InsertsBaseAfterHead_UsingPageUrlWithoutFragment()
    {
        var html = "<html><head class=\"client-nojs\"><title>Quest</title></head><body></body></html>";

        var document = WikiPageLoader.PrepareDocument(html, new Uri("https://wiki.guildwars.com/wiki/Quest#Rewards"));

        document.Should().StartWith("<html><head class=\"client-nojs\"><base href=\"https://wiki.guildwars.com/wiki/Quest\"><script>");
        document.Should().EndWith("<title>Quest</title></head><body></body></html>");
    }

    [TestMethod]
    public void PrepareDocument_WithoutHead_PrependsBase()
    {
        var document = WikiPageLoader.PrepareDocument("<p>Page</p>", new Uri("https://wiki.guildwars.com/wiki/Page"));

        document.Should().StartWith("<base href=\"https://wiki.guildwars.com/wiki/Page\">");
        document.Should().EndWith("</script><p>Page</p>");
    }

    [TestMethod]
    public void PrepareDocument_EncodesFragment_SoItCannotCloseTheScript()
    {
        var document = WikiPageLoader.PrepareDocument("<head></head>", new Uri("https://wiki.guildwars.com/wiki/Page"), "#</script><script>alert(1)");

        document.Should().NotContain("</script><script>alert(1)");
        document.Should().Contain("scrollToFragment(\"\\u003C/script\\u003E\\u003Cscript\\u003Ealert(1)\")");
    }

    [TestMethod]
    [DataRow("https://wiki.guildwars.com/wiki/Main_Page", true)]
    [DataRow("https://WIKI.GUILDWARS.COM/wiki/Main_Page", true)]
    [DataRow("http://wiki.guildwars.com/wiki/Main_Page", false)]
    [DataRow("https://wiki.guildwars.com.example.com/wiki/Main_Page", false)]
    [DataRow("https://gwpvx.fandom.com/wiki/Main_Page", false)]
    public void IsWikiUrl_OnlyAcceptsTheGuildWarsWikiOverHttps(string url, bool expected)
    {
        var loader = new WikiPageLoader(Substitute.For<IHttpClient<WikiPageLoader>>(), NullLogger<WikiPageLoader>.Instance);

        loader.IsWikiUrl(new Uri(url)).Should().Be(expected);
    }
}
