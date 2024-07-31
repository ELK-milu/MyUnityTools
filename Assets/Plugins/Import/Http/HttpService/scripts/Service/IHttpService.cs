using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHttpService
{
    string HandleHttpCommand(string rawUrl);
    int ServicePort { get; set; }
    string ServiceName { get; }
}
