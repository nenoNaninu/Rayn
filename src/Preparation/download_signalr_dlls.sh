#!/bin/bash
mkdir dlls/
wget https://www.nuget.org/api/v2/package/Microsoft.AspNetCore.SignalR.Client/5.0.5 -O Microsoft.AspNetCore.SignalR.Client.nupkg
unzip Microsoft.AspNetCore.SignalR.Client.nupkg -d Microsoft.AspNetCore.SignalR.Client
cp Microsoft.AspNetCore.SignalR.Client/lib/netstandard2.0/Microsoft.AspNetCore.SignalR.Client.dll dlls/Microsoft.AspNetCore.SignalR.Client.dll
wget https://www.nuget.org/api/v2/package/Microsoft.AspNetCore.Http.Connections.Client/5.0.5 -O Microsoft.AspNetCore.Http.Connections.Client.nupkg
unzip Microsoft.AspNetCore.Http.Connections.Client.nupkg -d Microsoft.AspNetCore.Http.Connections.Client
cp Microsoft.AspNetCore.Http.Connections.Client/lib/netstandard2.0/Microsoft.AspNetCore.Http.Connections.Client.dll dlls/Microsoft.AspNetCore.Http.Connections.Client.dll
wget https://www.nuget.org/api/v2/package/Microsoft.AspNetCore.Http.Connections.Common/5.0.5 -O Microsoft.AspNetCore.Http.Connections.Common.nupkg
unzip Microsoft.AspNetCore.Http.Connections.Common.nupkg -d Microsoft.AspNetCore.Http.Connections.Common
cp Microsoft.AspNetCore.Http.Connections.Common/lib/netstandard2.0/Microsoft.AspNetCore.Http.Connections.Common.dll dlls/Microsoft.AspNetCore.Http.Connections.Common.dll
wget https://www.nuget.org/api/v2/package/Microsoft.Extensions.Logging.Abstractions/5.0.0 -O Microsoft.Extensions.Logging.Abstractions.nupkg
unzip Microsoft.Extensions.Logging.Abstractions.nupkg -d Microsoft.Extensions.Logging.Abstractions
cp Microsoft.Extensions.Logging.Abstractions/lib/netstandard2.0/Microsoft.Extensions.Logging.Abstractions.dll dlls/Microsoft.Extensions.Logging.Abstractions.dll
wget https://www.nuget.org/api/v2/package/Microsoft.Extensions.Options/5.0.0 -O Microsoft.Extensions.Options.nupkg
unzip Microsoft.Extensions.Options.nupkg -d Microsoft.Extensions.Options
cp Microsoft.Extensions.Options/lib/netstandard2.0/Microsoft.Extensions.Options.dll dlls/Microsoft.Extensions.Options.dll
wget https://www.nuget.org/api/v2/package/Microsoft.Extensions.DependencyInjection.Abstractions/5.0.0 -O Microsoft.Extensions.DependencyInjection.Abstractions.nupkg
unzip Microsoft.Extensions.DependencyInjection.Abstractions.nupkg -d Microsoft.Extensions.DependencyInjection.Abstractions
cp Microsoft.Extensions.DependencyInjection.Abstractions/lib/netstandard2.0/Microsoft.Extensions.DependencyInjection.Abstractions.dll dlls/Microsoft.Extensions.DependencyInjection.Abstractions.dll
wget https://www.nuget.org/api/v2/package/Microsoft.Extensions.Primitives/5.0.1 -O Microsoft.Extensions.Primitives.nupkg
unzip Microsoft.Extensions.Primitives.nupkg -d Microsoft.Extensions.Primitives
cp Microsoft.Extensions.Primitives/lib/netstandard2.0/Microsoft.Extensions.Primitives.dll dlls/Microsoft.Extensions.Primitives.dll
wget https://www.nuget.org/api/v2/package/System.Buffers/4.5.1 -O System.Buffers.nupkg
unzip System.Buffers.nupkg -d System.Buffers
cp System.Buffers/lib/netstandard2.0/System.Buffers.dll dlls/System.Buffers.dll
wget https://www.nuget.org/api/v2/package/System.Memory/4.5.4 -O System.Memory.nupkg
unzip System.Memory.nupkg -d System.Memory
cp System.Memory/lib/netstandard2.0/System.Memory.dll dlls/System.Memory.dll
wget https://www.nuget.org/api/v2/package/System.Runtime.CompilerServices.Unsafe/5.0.0 -O System.Runtime.CompilerServices.Unsafe.nupkg
unzip System.Runtime.CompilerServices.Unsafe.nupkg -d System.Runtime.CompilerServices.Unsafe
cp System.Runtime.CompilerServices.Unsafe/lib/netstandard2.0/System.Runtime.CompilerServices.Unsafe.dll dlls/System.Runtime.CompilerServices.Unsafe.dll
wget https://www.nuget.org/api/v2/package/System.Numerics.Vectors/4.5.0 -O System.Numerics.Vectors.nupkg
unzip System.Numerics.Vectors.nupkg -d System.Numerics.Vectors
cp System.Numerics.Vectors/lib/netstandard2.0/System.Numerics.Vectors.dll dlls/System.Numerics.Vectors.dll
wget https://www.nuget.org/api/v2/package/System.Text.Json/5.0.2 -O System.Text.Json.nupkg
unzip System.Text.Json.nupkg -d System.Text.Json
cp System.Text.Json/lib/netstandard2.0/System.Text.Json.dll dlls/System.Text.Json.dll
wget https://www.nuget.org/api/v2/package/System.Text.Encodings.Web/5.0.1 -O System.Text.Encodings.Web.nupkg
unzip System.Text.Encodings.Web.nupkg -d System.Text.Encodings.Web
cp System.Text.Encodings.Web/lib/netstandard2.0/System.Text.Encodings.Web.dll dlls/System.Text.Encodings.Web.dll
wget https://www.nuget.org/api/v2/package/System.Threading.Tasks.Extensions/4.5.4 -O System.Threading.Tasks.Extensions.nupkg
unzip System.Threading.Tasks.Extensions.nupkg -d System.Threading.Tasks.Extensions
cp System.Threading.Tasks.Extensions/lib/netstandard2.0/System.Threading.Tasks.Extensions.dll dlls/System.Threading.Tasks.Extensions.dll
wget https://www.nuget.org/api/v2/package/Microsoft.AspNetCore.SignalR.Client.Core/5.0.5 -O Microsoft.AspNetCore.SignalR.Client.Core.nupkg
unzip Microsoft.AspNetCore.SignalR.Client.Core.nupkg -d Microsoft.AspNetCore.SignalR.Client.Core
cp Microsoft.AspNetCore.SignalR.Client.Core/lib/netstandard2.0/Microsoft.AspNetCore.SignalR.Client.Core.dll dlls/Microsoft.AspNetCore.SignalR.Client.Core.dll
wget https://www.nuget.org/api/v2/package/Microsoft.AspNetCore.SignalR.Common/5.0.5 -O Microsoft.AspNetCore.SignalR.Common.nupkg
unzip Microsoft.AspNetCore.SignalR.Common.nupkg -d Microsoft.AspNetCore.SignalR.Common
cp Microsoft.AspNetCore.SignalR.Common/lib/netstandard2.0/Microsoft.AspNetCore.SignalR.Common.dll dlls/Microsoft.AspNetCore.SignalR.Common.dll
wget https://www.nuget.org/api/v2/package/Microsoft.AspNetCore.SignalR.Protocols.Json/5.0.5 -O Microsoft.AspNetCore.SignalR.Protocols.Json.nupkg
unzip Microsoft.AspNetCore.SignalR.Protocols.Json.nupkg -d Microsoft.AspNetCore.SignalR.Protocols.Json
cp Microsoft.AspNetCore.SignalR.Protocols.Json/lib/netstandard2.0/Microsoft.AspNetCore.SignalR.Protocols.Json.dll dlls/Microsoft.AspNetCore.SignalR.Protocols.Json.dll
wget https://www.nuget.org/api/v2/package/Microsoft.Extensions.DependencyInjection/5.0.1 -O Microsoft.Extensions.DependencyInjection.nupkg
unzip Microsoft.Extensions.DependencyInjection.nupkg -d Microsoft.Extensions.DependencyInjection
cp Microsoft.Extensions.DependencyInjection/lib/netstandard2.0/Microsoft.Extensions.DependencyInjection.dll dlls/Microsoft.Extensions.DependencyInjection.dll
wget https://www.nuget.org/api/v2/package/Microsoft.Extensions.Logging/5.0.0 -O Microsoft.Extensions.Logging.nupkg
unzip Microsoft.Extensions.Logging.nupkg -d Microsoft.Extensions.Logging
cp Microsoft.Extensions.Logging/lib/netstandard2.0/Microsoft.Extensions.Logging.dll dlls/Microsoft.Extensions.Logging.dll
wget https://www.nuget.org/api/v2/package/System.Threading.Channels/5.0.0 -O System.Threading.Channels.nupkg
unzip System.Threading.Channels.nupkg -d System.Threading.Channels
cp System.Threading.Channels/lib/netstandard2.0/System.Threading.Channels.dll dlls/System.Threading.Channels.dll
wget https://www.nuget.org/api/v2/package/Microsoft.AspNetCore.Connections.Abstractions/5.0.5 -O Microsoft.AspNetCore.Connections.Abstractions.nupkg
unzip Microsoft.AspNetCore.Connections.Abstractions.nupkg -d Microsoft.AspNetCore.Connections.Abstractions
cp Microsoft.AspNetCore.Connections.Abstractions/lib/netstandard2.0/Microsoft.AspNetCore.Connections.Abstractions.dll dlls/Microsoft.AspNetCore.Connections.Abstractions.dll
wget https://www.nuget.org/api/v2/package/Microsoft.Bcl.AsyncInterfaces/5.0.0 -O Microsoft.Bcl.AsyncInterfaces.nupkg
unzip Microsoft.Bcl.AsyncInterfaces.nupkg -d Microsoft.Bcl.AsyncInterfaces
cp Microsoft.Bcl.AsyncInterfaces/lib/netstandard2.0/Microsoft.Bcl.AsyncInterfaces.dll dlls/Microsoft.Bcl.AsyncInterfaces.dll
wget https://www.nuget.org/api/v2/package/Microsoft.AspNetCore.Http.Features/5.0.5 -O Microsoft.AspNetCore.Http.Features.nupkg
unzip Microsoft.AspNetCore.Http.Features.nupkg -d Microsoft.AspNetCore.Http.Features
cp Microsoft.AspNetCore.Http.Features/lib/netstandard2.0/Microsoft.AspNetCore.Http.Features.dll dlls/Microsoft.AspNetCore.Http.Features.dll
wget https://www.nuget.org/api/v2/package/System.IO.Pipelines/5.0.1 -O System.IO.Pipelines.nupkg
unzip System.IO.Pipelines.nupkg -d System.IO.Pipelines
cp System.IO.Pipelines/lib/netstandard2.0/System.IO.Pipelines.dll dlls/System.IO.Pipelines.dll
wget https://www.nuget.org/api/v2/package/System.Diagnostics.DiagnosticSource/5.0.1 -O System.Diagnostics.DiagnosticSource.nupkg
unzip System.Diagnostics.DiagnosticSource.nupkg -d System.Diagnostics.DiagnosticSource
cp System.Diagnostics.DiagnosticSource/lib/netstandard2.0/System.Diagnostics.DiagnosticSource.dll dlls/System.Diagnostics.DiagnosticSource.dll
cp System.Diagnostics.DiagnosticSource/lib/netstandard1.3/System.Diagnostics.DiagnosticSource.dll dlls/System.Diagnostics.DiagnosticSource.dll
