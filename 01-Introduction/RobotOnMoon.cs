using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;


public class RobotOnMoon
{
    public string isSafeCommand(string[] board, string S)
    {
    public string isSafeCommand(string[] board, string S)
    {
        int initialPositionI = -1;
		int initialPositionJ = -1;
        for (int i = 0; i < board.Length; i++) {
            for (int j = 0 ; j < board[i].Length; j++ ) {
                if (board[i][j] == 'S') {
                    initialPositionI = i;
                    initialPositionJ = j;
                    break;
                }
            }
        }
        int currentPositionI = initialPositionI;
        int currentPositionJ = initialPositionJ;
        for (int i = 0; i < S.Length; i++) {
            int move = S[i];
            if (move == 'U') {
                if (currentPositionI < 1) {
                    return "Dead";
                }
                if (board[currentPositionI - 1][currentPositionJ] == '.' || board[currentPositionI - 1][currentPositionJ] == 'S') {
                    currentPositionI -= 1;
                }
            }
            if (move == 'D') {
                if (currentPositionI == board.Length - 1) {
                    return "Dead";
                }
                if (board[currentPositionI + 1][currentPositionJ] == '.' || board[currentPositionI + 1][currentPositionJ] == 'S') {
                    currentPositionI += 1;
                }
            }
            if (move == 'L') {
                if (currentPositionJ < 1) {
                    return "Dead";
                }
                if (board[currentPositionI][currentPositionJ - 1] == '.' || board[currentPositionI][currentPositionJ - 1] == 'S') {
                    currentPositionJ -= 1;
                }
            }
            if (move == 'R') {
                if (currentPositionJ == board[0].Length - 1) {
                    return "Dead";
                }
                if (board[currentPositionI][currentPositionJ + 1] == '.' || board[currentPositionI][currentPositionJ + 1] == 'S') {
                    currentPositionJ += 1;
                }
            }
        }
        return "Alive";
    }

    #region Testing code

    [STAThread]
    private static Boolean KawigiEdit_RunTest(int testNum, string[] p0, string p1, Boolean hasAnswer, string p2)
    {
        Console.Write("Test " + testNum + ": [" + "{");
        for (int i = 0; p0.Length > i; ++i)
        {
            if (i > 0)
            {
                Console.Write(",");
            }
            Console.Write("\"" + p0[i] + "\"");
        }
        Console.Write("}" + "," + "\"" + p1 + "\"");
        Console.WriteLine("]");
        RobotOnMoon obj;
        string answer;
        obj = new RobotOnMoon();
        DateTime startTime = DateTime.Now;
        answer = obj.isSafeCommand(p0, p1);
        DateTime endTime = DateTime.Now;
        Boolean res;
        res = true;
        Console.WriteLine("Time: " + (endTime - startTime).TotalSeconds + " seconds");
        if (hasAnswer)
        {
            Console.WriteLine("Desired answer:");
            Console.WriteLine("\t" + "\"" + p2 + "\"");
        }
        Console.WriteLine("Your answer:");
        Console.WriteLine("\t" + "\"" + answer + "\"");
        if (hasAnswer)
        {
            res = answer == p2;
        }
        if (!res)
        {
            Console.WriteLine("DOESN'T MATCH!!!!");
        }
        else if ((endTime - startTime).TotalSeconds >= 2)
        {
            Console.WriteLine("FAIL the timeout");
            res = false;
        }
        else if (hasAnswer)
        {
            Console.WriteLine("Match :-)");
        }
        else
        {
            Console.WriteLine("OK, but is it right?");
        }
        Console.WriteLine("");
        return res;
    }

    public static void Run()
    {
        Boolean all_right;
        all_right = true;

        string[] p0;
        string p1;
        string p2;

        // ----- test 0 -----
        p0 = new string[] {".....", ".###.", "..S#.", "...#."};
        p1 = "URURURURUR";
        p2 = "Alive";
        all_right = KawigiEdit_RunTest(0, p0, p1, true, p2) && all_right;
        // ------------------

        // ----- test 1 -----
        p0 = new string[] {".....", ".###.", "..S..", "...#."};
        p1 = "URURURURUR";
        p2 = "Dead";
        all_right = KawigiEdit_RunTest(1, p0, p1, true, p2) && all_right;
        // ------------------

        // ----- test 2 -----
        p0 = new string[] {".....", ".###.", "..S..", "...#."};
        p1 = "URURU";
        p2 = "Alive";
        all_right = KawigiEdit_RunTest(2, p0, p1, true, p2) && all_right;
        // ------------------

        // ----- test 3 -----
        p0 = new string[] {"#####", "#...#", "#.S.#", "#...#", "#####"};
        p1 = "DRULURLDRULRUDLRULDLRULDRLURLUUUURRRRDDLLDD";
        p2 = "Alive";
        all_right = KawigiEdit_RunTest(3, p0, p1, true, p2) && all_right;
        // ------------------

        // ----- test 4 -----
        p0 = new string[] {"#####", "#...#", "#.S.#", "#...#", "#.###"};
        p1 = "DRULURLDRULRUDLRULDLRULDRLURLUUUURRRRDDLLDD";
        p2 = "Dead";
        all_right = KawigiEdit_RunTest(4, p0, p1, true, p2) && all_right;
        // ------------------

        // ----- test 5 -----
        p0 = new string[] {"S"};
        p1 = "R";
        p2 = "Dead";
        all_right = KawigiEdit_RunTest(5, p0, p1, true, p2) && all_right;
        // ------------------

        if (all_right)
        {
            Console.WriteLine("You're a stud (at least on the example cases)!");
        }
        else
        {
            Console.WriteLine("Some of the test cases had errors.");
        }
    }

    #endregion
}