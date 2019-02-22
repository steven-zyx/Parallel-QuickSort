# Parallel-QuickSort  //Fix some bugs here
I used a slightly optimized sequential version of quicksort as basis, then added parallel functionality to it. The sequential version is from: Algorithms 4th edition.<br/>
After my experiment, the parallel version saves approximately 64% of the time compared with the sequential version, which is 9.2s compared with 25.6s under the CPU(E3-1230 v5) of 4 cores, 8 logical threads and 3.4GHz clocking rate. The data for sorting are 300M integers between 0 and 1M, which generated randomly using fixed seed.<br/>
Technically, I used continuation, and only assigned continuation if the subarrays have large enough length, which is 1/256 of the whole array. So it won't generate too much tasks for the TPL to schedule but retained a fine-grained control of the threads.<br/>
There is a problem bordering me, the most antecedent task doesn't receive anything when all its continuations finish. I can't predict how much continuation would be assigned. To deal with this problem, I had to trace the progress of the process and assign a thread to check the progress. This not only complicated the algorithm but also occupied an available Thread, which leads to bad performance.<br/>
I am looking forward to the solution as well as any suggestions.

Did some changes in branch iss53
Finished job on branch iss53