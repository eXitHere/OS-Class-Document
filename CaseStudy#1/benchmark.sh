#!/bin/sh

time1=$(dotnet run 1)
echo "1 Thread(s): $time1"

time2=$(dotnet run 2)
echo "2 Thread(s): $time2"

time3=$(dotnet run 4)
echo "4 Thread(s): $time3"
