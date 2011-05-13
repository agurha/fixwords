using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication3
{
    public class DAL
    {
        public IEnumerable<string> GetWords()
        {
            using (DataClasses1DataContext context = new DataClasses1DataContext())
            {
                var wordsDetails = context.Dim_Overs.Select(r => r);

                return wordsDetails.Where(r => r.displayed == false).Select(r => r.id).ToArray();
            }
        }

        public void WriteToOut(string idWord, List<string> generated)
        {
            using (DataClasses1DataContext context = new DataClasses1DataContext())
            {
                foreach (var VARIABLE in generated)
                {
                    Dim_Out goodOnes = new Dim_Out();
                    goodOnes.wd = idWord;
                    goodOnes.comb = VARIABLE;

                    

                    context.Dim_Outs.InsertOnSubmit(goodOnes);
                    context.SubmitChanges();
                }

             

            } 
        }

        public void UpdateStatus(string idWord)
        {
            using(DataClasses1DataContext context = new DataClasses1DataContext())
            {
                var over = (from i in context.Dim_Overs
                            where i.id == idWord
                            select i).Single();

                over.displayed = true;
                context.SubmitChanges();
            }
        }
    }
}
