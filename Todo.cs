public class Todo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    // not returned in the reponse body because using DTO 
    public string? Secret { get; set; }
}