public class SectionDto
{
    public int SectionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? SectionManagerId { get; set; }
    public string? SectionManagerFullName { get; set; }

}

public class CreateSectionDto
{
    public string Name { get; set; } = string.Empty;
    public int? SectionManagerId { get; set; }
}