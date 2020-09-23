pushd "%~dp0"

dotnet test --collect:"XPlat Code Coverage"

popd
