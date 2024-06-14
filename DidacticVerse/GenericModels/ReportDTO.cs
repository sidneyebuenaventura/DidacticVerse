using DidacticVerse.Enums;
using EfVueMantle;

namespace DidacticVerse.Models;

public class ReportDTO : DataTransferObjectBase
{
    public int Id { get; set; }
    public ReportReasons ReportReason { get; set; }
}
