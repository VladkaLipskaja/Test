using System;

namespace WebApplication1.Controllers
{
    internal class UrlClass
    {
        DateTime RDate   { get; set; }
        DateTime LRDate  { get; set; }
        string   URL     { get; set; }
        string   Title   { get; set; }
        string   LTitle  { get; set; }
        int      Status  { get; set; }
        int      LStatus { get; set; }
    }
}