using System.Text;
using AdventOfCodeCore.Models.Days;

namespace Problems._2024;

internal class Day9 : Day
{
    public override int Year => 2024;

    public override int DayNumber => 9;

    protected override string Part1()
    {
        var diskData = GetData();
        SortDiskData(ref diskData);
        return GetChecksum(diskData).ToString();
    }

    protected override string Part2()
    {
        var diskData = GetData();
        SortDiskDataByMovingFiles(ref diskData);
        return GetChecksum(diskData).ToString();
    }

    private void SortDiskDataByMovingFiles(ref List<int> diskData)
    {
        int last = diskData.Count - 1;
        int currentLastNumber =diskData.Last();

        while (last >= 0)
        {
            if (last <= 0)
                return;

            while (last > 0 && diskData[last] != currentLastNumber)
            {
                last--;
            }

            int length = 0;
            while (diskData[last] == currentLastNumber)
            {
                last--;
                length++;
                if (last <= 0)
                    break;
            }

            for (int i = 0; i < last; i++)
            {
                if (diskData[i] != -1)
                    continue;

                int firstSpace = i;

                while (i < diskData.Count && diskData[i] == -1)
                {
                    i++;
                }

                if (i - firstSpace < length)
                    continue;


                for (int j = firstSpace; j < firstSpace + length; j++)
                    diskData[j] = currentLastNumber;

                for (int k = last + 1; k < last + 1 + length; k++)
                    diskData[k] = -1;
                if (IsTest)
                {
                    var s = new string(IntArrayToString(diskData));
                    Log.Log(s);
                }
                break;
            }
            currentLastNumber--;
        }

        if (IsTest)
        {
            var s = new string(IntArrayToString(diskData));
            Log.Log(s);
        }
    }

    private void SortDiskData(ref List<int> diskData)
    {
        int last = diskData.Count - 1;
        int lastIndex = 0;
        for (int i = 0; i < diskData.Count; i++)
        {
            if (diskData[i] != -1)
                continue;

            while (diskData[last] == -1)
                last--;

            if (i >= last)
            {
                lastIndex = i;
                break;
            }

            diskData[i] = diskData[last];
            diskData[last] = -1;
            last--;
        }

        if (IsTest)
        {
            var s = new string(IntArrayToString(diskData));
            Log.Log(s);
        }

        Log.Log("Expected end: " + lastIndex);
    }

    private string IntArrayToString(List<int> ints)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var i in ints)
        {
            if (i != -1)
                sb.Append(i.ToString());
            else
                sb.Append('.');
        }
        return sb.ToString();
    }

    private long GetChecksum(List<int> diskData)
    {
        long checksum = 0;
        long count = 0;
        foreach (var c in diskData)
        {
            if (c == -1)
            {
                count++;
                continue;
            }
            checksum += c * count;
            count++;
        }

        return checksum;
    }

    private List<int> GetData()
    {
        List<int> diskData = [];
        bool checkId = true;
        int id = 0;
        foreach (var c in Input[0].Trim())
        {
            var val = int.Parse(c.ToString());
            if (checkId)
            {
                for (var i = 0; i < val; i++)
                    diskData.Add(id);

                id++;
            }
            else
            {
                for (int i = 0; i < val; i++)
                    diskData.Add(-1);
            }
            checkId = !checkId;
        }

        if (IsTest)
        {
            var s = new string(IntArrayToString(diskData));
            Log.Log(s);
        }

        return diskData;
    }
}