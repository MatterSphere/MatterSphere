using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("OMS Document Previewer Infrastructure")]
[assembly: AssemblyDescription("OMS Document Previewer Infrastructure")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("79c39a55-3ec3-4772-b325-7d9a07c5cf21")]


[assembly: InternalsVisibleTo("FWBS.DocumentPreviewer, PublicKey=" + Tokens.PublicKey)]
[assembly: InternalsVisibleTo("FWBS.DocumentPreviewer.Zip, PublicKey=" + Tokens.PublicKey)]
[assembly: InternalsVisibleTo("FWBS.DocumentPreviewer.Msg, PublicKey=" + Tokens.PublicKey)]

internal static class Tokens
{
    public const string PublicKey = "00240000048000009400000006020000002400005253413100040000010001001d6d320def528c823076862f317f5d2e40d70f71b8e4b0faf486c7108afc219227157bedcee9cf9d215be30245546c6c380f7472e729ede9cc36cbef9c5e9c04456749acc68a5242a05c0f54cdbb5e8bd20f50e2faf48b185afbd67c120daef17b22e6c91a86d37e18e2cd2e70c9dd648ee4fbcc4b48644663746ef80849e5b1";
}
