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

---

```c#
static void sum(int taskId, int start, int stop)
	{
		int index = start;
		Console.WriteLine("Thread {0} is working!", taskId);
		while(index != stop) {
			long data = Data_Global[index];
			long curSum = Sum_Global[taskId];
			if (data % 2 == 0)
			{
				curSum -= data;
			}
			else if (Data_Global[index] % 3 == 0)
			{
				curSum += (data*2);
			}
			else if (Data_Global[index] % 5 == 0)
			{
				curSum += (data / 2);
			}
			else if (Data_Global[index] %7 == 0)
			{
				curSum += (data / 3);
			}
			Sum_Global[taskId] = curSum;
			Data_Global[index] = 0;
			index += 1;
		}
	}
```

[MBP 16 2019]

| Threads | Time    |
| ------- | ------- |
| 1       | -       |
| 2       | -       |
| 4       | 20410ms |

คิดว่าแก้ function Sum ให้ไม่ต้อง access array บ่อยๆ (หลาย if) จะทำให้เร็วขึ้น
แต่ช้าลง อาจจะเป็นเพราะเสียเวลาข้ามตัวแปรไปมาบ่อยๆ

---

```c#
	static void sum(int taskId)
	{
		int index = taskId * (MAX/N);
		int stop  = (taskId+1) * (MAX/N);
		Console.WriteLine("Spawning thread {0,-5} Start {1,-10} Stop {2,-10}", taskId, index, stop);
		Console.WriteLine("Thread {0} is working!", taskId);
		while(index != stop) {
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
			Data_Global[index] = 0;
			index += 1;
		}
	}

	static void CreateThreads()
	{
		int nThread = lstThreads.Count;
		Thread th = new Thread(() => { sum(nThread); });
		th.Start();
		lstThreads.Add(th);
	}
```

[MBP 16 2019]

| Threads | Time    |
| ------- | ------- |
| 1       | -       |
| 2       | -       |
| 4       | 20410ms |

ลองปรับให้ฟังค์ชัน sum() รับตัวแปรน้อยลง อาจจะช่วยลดเวลาเพราะไม่ต้องโยกข้อมูลข้าม function call เยอะ
ผลลัพท์: ไม่ค่อยช่วยอะไรเท่าไร อาจจะเป็นเพราะไม่ได้เรียกใช้บ่อยมากหรือไม่ได้สำคัญมาก

---

พยายามลดการใช้ตัวแปร Global
เอา Global_Sum ออก แล้วเปลี่ยนมาใช้ Local แทน

```c#
static void sum(int taskId, int start, int stop)
{
	int index = start;
	int sum = 0;
	Console.WriteLine("Thread {0} is working!", taskId);
	while(index != stop) {
		if (Data_Global[index] % 2 == 0)
		{
			sum -= Data_Global[index];
		}
		else if (Data_Global[index] % 3 == 0)
		{
			sum += (Data_Global[index]*2);
		}
		else if (Data_Global[index] % 5 == 0)
		{
			sum += (Data_Global[index] / 2);
		}
		else if (Data_Global[index] %7 == 0)
		{
			sum += (Data_Global[index] / 3);
		}
		Data_Global[index] = 0;
		index += 1;
	}
	Sum_Global += sum;
}
```

output

```
starting benchmark!

cpu model name : AMD Ryzen 7 3750H with Radeon Vega Mobile Gfx
Fri Oct  8 12:58:55 +07 2021
----------------------------------------------------------------------------
Test  1  Threads
round 1  timeusage: 23004 888701676
round 2  timeusage: 23497 888701676
round 3  timeusage: 23097 888701676
avg.  23199  ms

----------------------------------------------------------------------------
Test  2  Threads
round 1  timeusage: 12281 888701676
round 2  timeusage: 12819 888701676
round 3  timeusage: 12010 888701676
avg.  12370  ms

----------------------------------------------------------------------------
Test  4  Threads
round 1  timeusage: 6329 888701676
round 2  timeusage: 7044 888701676
round 3  timeusage: 6675 888701676
avg.  6682  ms
```
