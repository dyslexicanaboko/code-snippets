# 06/01/2024

This code example is based off of the same ones referenced by the [official gRPC website](https://grpc.io/docs/languages/csharp/):
[Tutorial: Create a gRPC client and server in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start)

I implemented the Greeter example as per the instructions and then took it a step further to apply my own example of adding two numbers and producing a result. I feel that this is a better practice example over the casual `Hello World` example.

## General notes

- It's not immediately obvious, but in order for a `*.proto` file to be built, you have to manually add it to the `*.csproj` project definition as part of an `ItemGroup`.
- You can continue to add includes to the same `ItemGroup`.
- The `GrpcServices` attribute value must match project type it is in. In other words, use `Server` for your server project and use `Client` for your client project.
- Your `*.proto` must be identical between projects.
- Your `*.proto` files are used to produce your class/model definitions and can be seen in the object folder's output: `~\obj\Debug\net8.0\Protos`. Do not modify the contents of these files! Anything you do must be modeled using Protobuf.

### Protobuf ItemGroup for Server

```xml
<ItemGroup>
  <Protobuf Include="Protos\definition01.proto" GrpcServices="Server" />
  <Protobuf Include="Protos\definition02.proto" GrpcServices="Server" />
</ItemGroup>
```

### Protobuf ItemGroup for Client

```xml
<ItemGroup>
  <Protobuf Include="Protos\definition01.proto" GrpcServices="Client" />
  <Protobuf Include="Protos\definition02.proto" GrpcServices="Client" />
</ItemGroup>
```
