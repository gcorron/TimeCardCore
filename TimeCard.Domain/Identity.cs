using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TimeCard.Domain;
public class Identity
{
    public string? Token { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string UserFullName { get; set; }
    public int ContractorId { get; set; }
    public int UserContractorId { get; set; }
    public string ContractorName { get; set; }
    public IEnumerable<string> Roles { get; set; }
    public int ExpireMinutes { get; set; }
}
