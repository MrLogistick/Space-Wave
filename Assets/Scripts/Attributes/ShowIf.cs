using System;

public class ShowIfAttribute : Attribute
{
    public string ConditionFieldName;

    public ShowIfAttribute(string conditionFieldName)
    {
        ConditionFieldName = conditionFieldName;
    }
}