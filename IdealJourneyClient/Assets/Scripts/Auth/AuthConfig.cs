using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AuthConfig
{
#if USE_LOCAL_SERVER
    public static readonly string ApiServerRoot = "http://localhost:8081/";
#else
    public static readonly string ApiServerRoot = "https://ideal-journey.herokuapp.com/";
#endif
}
