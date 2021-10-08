#!/bin/sh
reset

now=`date`
echo "\r\nstarting benchmark!\r\n"
echo "cpu" $(cat /proc/cpuinfo | grep "model name" -m 1)
echo "$now"
echo "\\r\\nCurrent date: $now " >> output.txt

for thread in 1 2 4; 
do
    echo "----------------------------------------------------------------------------"
    echo "Test " $thread " Threads"
    sum=0;
    for i in $(seq 1 3); 
    do 
        echo -n "round" $i " ";
        time1=$(dotnet run $thread);
        cur_time=$(cat output.txt);
        sum=$((cur_time+sum));
        echo timeusage: $cur_time ms;
    done
    echo "avg. " $((sum/3)) " ms\r\n";
done