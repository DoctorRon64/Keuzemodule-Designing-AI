using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskStatus { Success, Failed, Running }
public abstract class BTBaseNode
{
    public abstract TaskStatus Run();
}
