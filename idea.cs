// nolaunchernag -c|q "c:\path\original_run.exe"

class ProcessList
{
    int parent_pid;
    List<ProcessList> childs;


    void FollowTheTrail(int process_pid, string process_name)
    {
        var child_pids = new List<int>();

        boolean found_new = true;
        while (found_new)
        {
            found_new = false;
            foreach (var pid in list)
            {
                var new_list = GetChildPid(pid);

                if (new_list.length > 0)
                {
                    found_new = true;
                    list.add(new_list);
                }
            }
        }
    }


    public void GetChildPid()
    {
        using (var searcher = new ManagementObjectSearcher(String.Format(
            "SELECT ProcessId FROM Win32_Process WHERE ParentProcessId = {0}", parent_pid)))
        {
            
            foreach (var result in searcher.Get())
            {
                try
                {
                    var result_pid = Convert.ToInt32(result["ProcessId"]);

                    var child = new ProcessList(result_pid);
                    this.childs.add(child);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }
    }
}



