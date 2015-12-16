using UnityEngine;
using System.Collections;
using System.Diagnostics;

using System.IO;
//using System.IO.Pipes;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

public class BalanceBoardScript : MonoBehaviour
{

    public float deadzone;

    // Use this for initialization
    void Start()
    {

        //try
        //{
        foreach(Process proc in Process.GetProcessesByName("WiiBalanceWalker"))
            {
                proc.Kill();
                UnityEngine.Debug.Log("PROZESS GEKILLT!");
            }
        //}
        //catch(Exception ex)
        //{
        //    MessageBox.Show(ex.Message);
        //}


        startExternalApplication(@"D:\Dragonfly\WiiBalanceWalker_v0.4\WiiBalanceWalker_v0.4\WiiBalanceWalker\bin\Debug\WiiBalanceWalker.exe");
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetKeyDown(KeyCode.Return))
        {
            UnityEngine.Debug.Log("reset");
            this.gameObject.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        }


         try
         {
             string weight = System.IO.File.ReadAllText(@"D:\rwWT.txt");
             float weightFloat = float.Parse(weight);

             string tl = System.IO.File.ReadAllText(@"D:\rwTL.txt");
             float tlFloat = float.Parse(tl);

             string bl = System.IO.File.ReadAllText(@"D:\rwBL.txt");
             float blFloat = float.Parse(bl);

             string tr = System.IO.File.ReadAllText(@"D:\rwTR.txt");
             float trFloat = float.Parse(tr);

             string br = System.IO.File.ReadAllText(@"D:\rwBR.txt");
             float brFloat = float.Parse(br);

             weightFloat = tlFloat + trFloat + blFloat + brFloat;

             float tlpercent = tlFloat / weightFloat;
             float trpercent = trFloat / weightFloat;
             float blpercent = blFloat / weightFloat;
             float brpercent = brFloat / weightFloat;


             float horizontal = ((trpercent + brpercent) / 2) - ((tlpercent + blpercent) / 2);
             float vertical = ((trpercent + tlpercent) / 2) - ((brpercent + blpercent) / 2);

             if(horizontal < deadzone && horizontal > -deadzone)
             {
                 horizontal = 0.0f;
             }
             if(vertical < deadzone && vertical > -deadzone)
             {
                 vertical = 0.0f;
             }
             // float horizontal2 = ((((trFloat + brFloat) - (tlFloat + blFloat)) / 2) / ((tlFloat + trFloat + blFloat + brFloat) / 4)) / 2;

             this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + horizontal * 0.4f, this.gameObject.transform.position.y, this.gameObject.transform.position.z + vertical * 0.4f);
         }
         catch(IOException e)
         {

         }



    }

    static void startExternalApplication(string filename)
    {
        // String path = @"f:\temp\data.txt";
        Process process = new Process();
        process.StartInfo.FileName = filename;
        //foo.StartInfo.Arguments = path;
        process.Start();
    }

    //static void StartServer()
    //{
    //    Task.Factory.StartNew(() =>
    //    {
    //        var server = new NamedPipeServerStream("DragonPipe");
    //        server.WaitForConnection();
    //        StreamReader reader = new StreamReader(server);
    //        StreamWriter writer = new StreamWriter(server);
    //        while(true)
    //        {
    //            var line = reader.ReadLine();
    //            writer.WriteLine(String.Join("", line.Reverse()));
    //            writer.Flush();
    //        }
    //    });
    //}
}