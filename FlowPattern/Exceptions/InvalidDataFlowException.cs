using System;

namespace FlowPattern.Exceptions;

public class InvalidDataFlowException : Exception
{
    private static InvalidDataFlowException defaultException = new InvalidDataFlowException();
    public static InvalidDataFlowException Default => defaultException;
    public override string Message => 
        "O tipo de dado recebido do fluxo é inválido.";
}