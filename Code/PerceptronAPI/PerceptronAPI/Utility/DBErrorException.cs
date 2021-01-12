using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Utility
{
    public class DBErrorException
    {
        public void DbEntitiyError(DbEntityValidationException e)  //Courtesy by https://www.codeproject.com/Questions/1012001/VALIDATION-FAILED-FOR-ONE-OR-MORE-ENTITIES-SEE-ENT
        {
            foreach (var eve in e.EntityValidationErrors)
            {
                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        ve.PropertyName, ve.ErrorMessage);
                }
            }
        }
    }
}