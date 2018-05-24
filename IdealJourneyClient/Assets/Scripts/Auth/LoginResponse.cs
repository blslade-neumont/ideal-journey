using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class LoginResponse
{
    public string authToken;
    public User user;
}
