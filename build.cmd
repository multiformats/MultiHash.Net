@echo off
powershell -NoProfile -ExecutionPolicy RemoteSigned -Command ".\build.ps1 -Configuration" %1