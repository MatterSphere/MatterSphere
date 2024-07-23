using System;
namespace Fwbs.Documents
{
    public interface IPropertyConverter<TSourceType, TSourceValue>
    {
        object FromSource(TSourceValue value, TSourceType type);
        TSourceValue ToSource(object value);

        Type ConvertType(Type type);
        Type FromSourceType(TSourceType type);
        TSourceType ToSourceType(Type type);
    }
}
