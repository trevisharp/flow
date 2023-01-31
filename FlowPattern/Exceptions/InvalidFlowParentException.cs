using System;

namespace FlowPattern.Exceptions;

public class InvalidFlowParentException : Exception
{
    private static InvalidFlowParentException defaultException = new InvalidFlowParentException();
    public static InvalidFlowParentException Default => defaultException;
    public override string Message => 
        "O pai deste fluxo não é um fluxo e não pode iniciar";
}