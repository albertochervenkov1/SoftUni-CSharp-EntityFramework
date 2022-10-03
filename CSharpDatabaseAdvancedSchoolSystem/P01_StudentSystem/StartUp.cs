using System;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data;

namespace P01_StudentSystem
{
    public class StartUp
    {
        static void Main(string[] args)
        {
           StudentSystemContext dbContext=new StudentSystemContext();
          dbContext.Database.Migrate();
           
        }
    }
}
