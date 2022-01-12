@echo off
dotnet new --uninstall RocketMod.Modern
dotnet new --install %1
exit