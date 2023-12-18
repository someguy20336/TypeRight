using Microsoft.Build.Framework;
using System;

namespace TypeRight.Build;

public class TestTask : ITask
{
    public IBuildEngine BuildEngine { get; set; }
    public ITaskHost HostObject { get; set; }

    [Required]
    public ITaskItem[] ReferencePath { get; set; }

    [Required]
    public ITaskItem[] Compile { get; set; }

    [Required]
    public ITaskItem BaseDirectory { get; set; }

    public bool Execute()
    {
        return true;
    }
}
