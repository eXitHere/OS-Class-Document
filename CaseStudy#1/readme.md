answer: 888701676
time: 24994ms

ทดสอบการทำงานที่ 4 Thread

```C#
 static void task(int taskId, int start, int stop) {
 	int i = start;
 		for(; i<stop; i++) {
 		// Console.WriteLine(i);
			sum(i, taskId);
 		}
 		Console.WriteLine(i);
 	}

 static void sum(int index, int taskId) {
	 if (Data_Global[index] % 2 == 0)
	 {
	 Sum_Global[taskId] -= Data_Global[index];
	 }
	 else if (Data_Global[index] % 3 == 0)
	 {
	 	Sum_Global[taskId] += (Data_Global[index]*2);
	 }
	 else if (Data_Global[index] % 5 == 0)
	 {
	 	Sum_Global[taskId] += (Data_Global[index] / 2);
	 }
	 else if (Data_Global[index] %7 == 0)
	 {
	 	Sum_Global[taskId] += (Data_Global[index] / 3);
 	}
 // Data_Global[index] = 0;
 // G_index++;
 }
```

| Threads | Time    |
| ------- | ------- |
| 1       | 24994ms |
| 2       | 17038ms |
| 4       | 12596ms |

ปรับแก้เอา function task ออก แล้วให้ loop ใน sum เลย
: พยายามลด For loop และการเรียกใช้ function

```c#
static void sum(int taskId, int start, int stop) {
	 int index = start;
	 while(index != stop) {
		 if (Data_Global[index] % 2 == 0)
		 {
			Sum_Global[taskId] -= Data_Global[index];
		 }
		 else if (Data_Global[index] % 3 == 0)
		 {
			Sum_Global[taskId] += (Data_Global[index]*2);
		 }
		 else if (Data_Global[index] % 5 == 0)
		 {
			Sum_Global[taskId] += (Data_Global[index] / 2);
		 }
		 else if (Data_Global[index] %7 == 0)
		 {
			Sum_Global[taskId] += (Data_Global[index] / 3);
		 }
		 index += 1;
	 }
	 // Data_Global[index] = 0;
	 // G_index++;
 }
```

| Threads | Time    |
| ------- | ------- |
| 1       | 24940ms |
| 2       | 12686ms |
| 4       | 9789ms  |
