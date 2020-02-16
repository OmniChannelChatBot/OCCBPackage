using Microsoft.AspNetCore.Routing;

namespace OCCBPackage.Mvc.ParameterTransformers
{
    public class CamelCaseParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value) =>
           (value as string)?.ToLower() ?? default;
    }
}
