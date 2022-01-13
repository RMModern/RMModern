@echo off
dotnet new --uninstall RocketMod.Modern.Generator
dotnet new --install %1
exit