using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simplex
{
    class Program
    {

        /// <summary>
        /// 
        /// </summary>
        static int zm, obm;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            bool end = false;
            do
            {

                Console.WriteLine("Введiть к-ть змiнних:");
                zm = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введiть к-ть обмежень:");
                obm = Convert.ToInt32(Console.ReadLine());
                Fraction[] Fx = new Fraction[zm];
                Console.WriteLine("Введiть ф-цiю цiлi:");
                for (int i = 0; i < Fx.Length; i++)
                {
                    Console.WriteLine("Kоефiцiєнт " + (i + 1));
                    string st = Console.ReadLine();
                    if (st.Contains("/"))
                    {
                        string[] s = st.Split('/');
                        Fx[i] = new Fraction(Convert.ToDouble(s[0]), Convert.ToDouble(s[1]));
                    }
                    else
                        Fx[i] = new Fraction(Convert.ToDouble(st));
                }







                Console.WriteLine("Ф-цiя цiлi мiнiмiзується чи максмiзується?min/max");
                string str = Console.ReadLine();
                bool ismax = false;
                if (str == "max")
                {
                    ismax = true;
                    for (int i = 0; i < Fx.Length; i++)
                        Fx[i] = Fx[i] * (-1);
                }
                Console.WriteLine();







                double[,] matr = new double[obm, zm];
                Fraction[,] matr1 = new Fraction[obm, zm];
                double[] arr = new double[zm];
                for (int i = 0; i < matr1.GetLength(0); i++)
                {
                    Console.WriteLine("Введiть обмеження " + (i + 1));
                    for (int k = 0; k < matr1.GetLength(1); k++)
                    {
                        Console.WriteLine("Kоефiцiєнт " + (k + 1));
                        string st = Console.ReadLine();
                        if (st.Contains("/"))
                        {
                            string[] s = st.Split('/');
                            matr1[i, k] = new Fraction(Convert.ToDouble(s[0]), Convert.ToDouble(s[1]));
                        }
                        else
                            matr1[i, k] = new Fraction(Convert.ToDouble(st));
                    }
                }






                string str1 = "";
                Console.WriteLine("Введiть знаки обмежень");
                str1 = Console.ReadLine();
                string[] znaki = str1.Split(' ');






                Console.WriteLine("Введiть стовпчик вiльних членiв");
                Fraction[] vilni = new Fraction[obm];
                for (int i = 0; i < vilni.Length; i++)
                {
                    Console.WriteLine("Введiть коефiцiєнт " + (i + 1));
                    string st = Console.ReadLine();
                    if (st.Contains("/"))
                    {
                        string[] s = st.Split('/');
                        vilni[i] = new Fraction(Convert.ToDouble(s[0]), Convert.ToDouble(s[1]));
                    }
                    else
                        vilni[i] = new Fraction(Convert.ToDouble(st));
                }





                int infinity = 0;
                Fraction[,] convertedMatr = new Fraction[obm, zm + obm];
                convertedMatr = convertToAvailable(ref matr1, znaki, ref vilni, ref infinity);
                Fraction[] additionalFx = new Fraction[convertedMatr.GetLength(1)];
                for (int j = 0; j < additionalFx.Length; j++)
                {
                    additionalFx[j] = new Fraction(0);
                }
                for (int j = 0; j < Fx.Length; j++)
                {
                    additionalFx[j] = Fx[j];
                }
                if (znaki[0] == ">=")
                    additionalFx[infinity] = new Fraction(10000);




                int[] basis = new int[obm];
                int count1 = 0;
                for (int i = 0; i < convertedMatr.GetLength(0); i++)
                {
                    for (int j = 0; j < convertedMatr.GetLength(1); j++)
                    {
                        if (convertedMatr[i, j].numerator == 1.0 && convertedMatr[i, j].denominator == 1.0)
                        {
                            for (int m = 0; m < convertedMatr.GetLength(0); m++)
                            {
                                if (convertedMatr[m, j].numerator == 0.0)
                                    count1++;
                            }
                            if (count1 == convertedMatr.GetLength(0) - 1)
                                basis[i] = j;
                            count1 = 0;
                        }
                    }
                }






                //ocinki
                Fraction[] ocinki = new Fraction[convertedMatr.GetLength(1)];
                for (int j = 0; j < convertedMatr.GetLength(1); j++)
                {
                    ocinki[j] = new Fraction(0);
                }
                for (int j = 0; j < convertedMatr.GetLength(1); j++)
                {
                    for (int k = 0; k < convertedMatr.GetLength(0); k++)
                    {
                        ocinki[j] = ocinki[j] + (convertedMatr[k, j] * additionalFx[basis[k]]);
                    }
                    ocinki[j] = ocinki[j] - additionalFx[j];
                }





                //bool isbad = false;
                //int count = 0;
                //for (int i = 0; i < ocinki.Length; i++)
                //{
                //    if (ocinki[i] > 0)
                //        isbad = true;
                //}

                //for (int i = 0; i < vilni.Length; i++)
                //{
                //    if (vilni[i] > 0)
                //        count++;
                //    if (count == vilni.Length - 1)
                //        isbad = true;
                //}

                //if (isbad == true)
                //    Console.WriteLine("Неможливо розвязувати двоїстим симплекс методом");




                //point

                Fraction sum = new Fraction(0);
                for (int i = 0; i < basis.Length; i++)
                {
                    sum = sum + (vilni[i] * additionalFx[basis[i]]);
                }



                showTable(convertedMatr, vilni, basis, sum, ocinki);
                bool exit = false;
                while (exit == false)
                {
                    int res = calculate(convertedMatr, ocinki, vilni);
                    switch (res)
                    {
                        case 1:
                            //Fraction[] x = new Fraction[convertedMatr.GetLength(1)];
                            //for (int i = 0; i < x.Length; i++)
                            //    x[i] = new Fraction(0);
                            //for (int i = 0; i < basis.Length; i++)
                            //{
                            //    x[basis[i]] = vilni[i];
                            //}
                            //Console.Write("x(");
                            //for (int i = 0; i < x.Length; i++)
                            //{
                            //    if (x[i].sign == -1)
                            //        Console.Write("-");
                            //    x[i].Reduce();
                            //    if (x[i].denominator == 1.0)
                            //        Console.Write(x[i].numerator + "; ");
                            //    else
                            //        Console.Write(x[i].numerator + "/" + x[i].denominator + "; ");
                            //}
                            //if (sum.sign == -1)
                            //{
                            //    Console.Write(")     f(x)=-" + sum.numerator + "/" + sum.denominator);
                            //}
                            //else
                            //    if (sum.denominator == sum.numerator)
                            //    Console.Write(")     f(x)=" + 1);
                            //if (sum.denominator == 1)
                            //    Console.Write(")     f(x)=" + sum.numerator);
                            //else
                            //    Console.Write(")     f(x)=" + sum.numerator + "/" + sum.denominator);


                            //if (ismax == true)
                            //    sum.sign *= -1;
                            //Fraction[] x1 = new Fraction[zm];
                            //for (int i = 0; i < x1.Length; i++)
                            //{
                            //    x1[i] = x[i];
                            //}
                            //Console.WriteLine();
                            //Console.Write("x*(");
                            //for (int i = 0; i < x1.Length; i++)
                            //{
                            //    if (x1[i].sign == -1)
                            //        Console.WriteLine("-");
                            //    x1[i].Reduce();
                            //    if (x1[i].denominator == 1.0)
                            //        Console.Write(x1[i].numerator + "; ");
                            //    else
                            //        Console.Write(x1[i].numerator + "/" + x1[i].denominator + "; ");
                            //}
                            //if (sum.sign == -1)
                            //{
                            //    Console.Write(")     f(x*)=-" + sum.numerator + "/" + sum.denominator);
                            //}
                            //else
                            //    if (sum.denominator == sum.numerator)
                            //    Console.Write(")     f(x*)=" + 1);
                            //if (sum.denominator == 1)
                            //    Console.Write(")     f(x*)=" + sum.numerator);
                            //else
                            //    Console.Write(")     f(x*)=" + sum.numerator + "/" + sum.denominator);
                            //
                            int indexOfCut = selectMaxPart(convertedMatr, vilni, basis, Fx);
                            if (indexOfCut == -1)
                            {
                                //vivod reshenia

                            }
                            else
                            {
                                calculateDoubleSimplex(ref convertedMatr, ref ocinki, ref vilni, ref basis, ref sum, ref additionalFx);
                            }
                            exit = true;
                            break;
                        case 2:
                            createNewBasisForward(ref convertedMatr, ref vilni, ref sum, ref ocinki, ref basis, additionalFx);
                            showTable(convertedMatr, vilni, basis, sum, ocinki);
                            Console.WriteLine();
                            break;
                        case 3:
                            Console.WriteLine("Ф-цiя цiлi необмежена знизу на МПР");
                            exit = true;
                            break;
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Розвязати ще приклад? y/n");
                string str2 = Console.ReadLine();
                if (str2 == "y")
                    Console.Clear();
                else
                    end = true;
            }
            while (end == false);
            Console.ReadKey();
        }

        private static int checkDoubleSimplex(Fraction[,] extendedMatrix, Fraction[] ocinki, Fraction[] vilni)
        {
            int res = 0;
            int count = 0, count2 = 0;
            bool isAvailable = false;
            for (int i = 0; i < vilni.Length; i++)
            {
                if (vilni[i].sign == 1)
                {
                    count++;
                    if (count == vilni.Length)
                    {
                        return res = 1;
                    }
                }
                else
                {
                    count2 = 0;
                    for (int j = 0; j < extendedMatrix.GetLength(1); j++)
                    {
                        if (extendedMatrix[i, j].sign == 1)
                        {
                            count2++;
                            if (count2 == extendedMatrix.GetLength(1))
                                return res = 3;
                        }

                    }

                }
            }
            return res = 2;
        }

        private static void calculateDoubleSimplex(ref Fraction[,] extendedMatrix, ref Fraction[] ocinki, ref Fraction[] vilni, ref int[] basis, ref Fraction sum, ref Fraction[] fx)
        {
            int indexOfCut = selectMaxPart(extendedMatrix, vilni, basis, fx);
            Fraction[,] extendedMatrix1 = makeNewCut(ref extendedMatrix, ref vilni, ref basis, ref indexOfCut, ref ocinki, ref fx);
            showTable(extendedMatrix1, vilni, basis, sum, ocinki);
            bool exit = false;
            while (exit == false)
            {
                int q = checkDoubleSimplex(extendedMatrix1, ocinki, vilni);
                switch (q)
                {
                    case 1:
                        Console.WriteLine("GOOOD");
                        int index = selectMaxPart(extendedMatrix1, vilni, basis, fx);
                        if (index != -1)
                            calculateDoubleSimplex(ref extendedMatrix1, ref ocinki, ref vilni, ref basis, ref sum, ref fx);
                        exit = true;
                        break;
                    case 2:
                        createNewBasisDouble(ref extendedMatrix, ref vilni, ref sum, ref ocinki, ref basis);
                        showTable(extendedMatrix1, vilni, basis, sum, ocinki);
                        break;
                    case 3:
                        Console.WriteLine("safsdokg");
                        break;
                }
            }

        }

        private static void isBad(Fraction[,] extendedMatrix1)
        {
            throw new NotImplementedException();
        }

        private static void createNewBasisDouble(ref Fraction[,] extendedMatrix, ref Fraction[] vilni, ref Fraction sum, ref Fraction[] ocinki, ref int[] basis)
        {
            Fraction minViln = new Fraction(0);
            int min = 0, min1 = 0;
            for (int i = 0; i < vilni.Length; i++)
            {
                if (vilni[i] < minViln)
                {
                    minViln = vilni[i];
                    min = i;
                }
            }

            Fraction minOcinki = new Fraction(Double.MaxValue);
            for (int i = 0; i < extendedMatrix.GetLength(1); i++)
            {
                if (extendedMatrix[min, i].sign == -1)
                {
                    if (ocinki[i] / extendedMatrix[min, i] < minOcinki)
                    {
                        minOcinki = ocinki[i] / extendedMatrix[min, i];
                        min1 = i;
                    }
                }
            }

            basis[min] = min1;
            Fraction key = extendedMatrix[min, min1];
            Fraction key2 = ocinki[min1];

            key2.sign = key2.sign * (-1);
            for (int i = 0; i < extendedMatrix.GetLength(1); i++)
                extendedMatrix[min, i] /= key;
            vilni[min] /= key;
            for (int i = 0; i < extendedMatrix.GetLength(0); i++)
            {
                if (i != min)
                {
                    key = extendedMatrix[i, min1];
                    if (key.sign == 1)
                        key.sign = -1;
                    else
                        key.sign = 1;
                    vilni[i] = vilni[i] + (vilni[min] * key);
                    for (int j = 0; j < extendedMatrix.GetLength(1); j++)
                    {
                        if (i != min)
                        {
                            if (j == min1)
                                extendedMatrix[i, j] = new Fraction(0);
                            else
                                extendedMatrix[i, j] = extendedMatrix[i, j] + (extendedMatrix[min, j] * key);
                        }
                    }
                }
            }

            for (int i = 0; i < ocinki.Length; i++)
            {
                if (i == min1)
                    ocinki[i] = new Fraction(0);
                else
                    ocinki[i] = ocinki[i] + (extendedMatrix[min, i] * key2);
            }

            key = extendedMatrix[min, min1];
            sum += vilni[min] * key;

        }

        private static Fraction[,] makeNewCut(ref Fraction[,] convertedMatr, ref Fraction[] vilni, ref int[] basis, ref int indexOfCut, ref Fraction[] ocinki, ref Fraction[] fx)
        {
            Fraction[,] extendedMatr = new Fraction[convertedMatr.GetLength(0) + 1, convertedMatr.GetLength(1) + 1];
            for (int i = 0; i < extendedMatr.GetLength(0); i++)
            {
                for (int j = 0; j < extendedMatr.GetLength(1); j++)
                {
                    extendedMatr[i, j] = new Fraction(0);
                }
            }
            for (int i = 0; i < convertedMatr.GetLength(0); i++)
            {
                for (int j = 0; j < convertedMatr.GetLength(1); j++)
                {
                    extendedMatr[i, j] = convertedMatr[i, j];
                }
            }

            Fraction[] S = new Fraction[convertedMatr.GetLength(1)];
            int q = 0;
            for (int j = 0; j < convertedMatr.GetLength(1); j++)
            {
                S[q] = new Fraction(-((convertedMatr[indexOfCut, j].numerator % convertedMatr[indexOfCut, j].denominator) / convertedMatr[indexOfCut, j].denominator));
                q++;
            }
            Fraction newViln = new Fraction(-((vilni[indexOfCut].numerator % vilni[indexOfCut].denominator) / vilni[indexOfCut].denominator));
            for (int j = 0; j < convertedMatr.GetLength(1); j++)
            {
                extendedMatr[extendedMatr.GetLength(0) - 1, j] = S[j];
            }
            extendedMatr[extendedMatr.GetLength(0) - 1, extendedMatr.GetLength(1) - 1] = new Fraction(1);
            Array.Resize<Fraction>(ref vilni, vilni.Length + 1);
            vilni[vilni.Length - 1] = newViln;
            Array.Resize<Fraction>(ref ocinki, ocinki.Length + 1);
            ocinki[ocinki.Length - 1] = new Fraction(0);
            Array.Resize<Fraction>(ref fx, fx.Length + 1);
            fx[fx.Length - 1] = new Fraction(0);
            return extendedMatr;
        }

        private static int selectMaxPart(Fraction[,] convertedMatr, Fraction[] vilni, int[] basis, Fraction[] fx)
        {
            int index = 0, count = 0, count1=0;

            int[] cili = new int[basis.Length];
            for (int i = 0; i < basis.Length; i++)
            {
                if (basis[i] < fx.Length)
                {
                    cili[count] = i;
                    count++;
                }
            }

            Fraction max = new Fraction(double.MinValue);
            Fraction secondmax = new Fraction(double.MinValue);
            Fraction[] parts = new Fraction[count];
            for (int i = 0; i < count; i++)
            {
                if (vilni[cili[i]].numerator % vilni[cili[i]].denominator != 0)
                {
                    parts[i] = new Fraction((vilni[cili[i]].numerator % vilni[cili[i]].denominator) / vilni[cili[i]].denominator);
                    count1++;
                }
            }
            if (count1 == 0)
            {
                return index = -1;
            }
            else
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i] > max)
                        index = i;
                }
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i] == max)
                    {
                        Fraction sum = new Fraction(0);
                        for (int j = 0; j < convertedMatr.GetLength(1); j++)
                        {
                            sum = sum + new Fraction((convertedMatr[cili[i], j].numerator % convertedMatr[cili[i], j].denominator) / convertedMatr[cili[i], j].denominator);
                        }
                        if (new Fraction((vilni[cili[i]].numerator % vilni[cili[i]].denominator) / vilni[cili[i]].denominator) / sum > secondmax)
                        {
                            secondmax = new Fraction((vilni[cili[i]].numerator % vilni[cili[i]].denominator) / vilni[cili[i]].denominator) / sum;
                            index = i;
                        }
                    }
                }
                return index;
            }
        }

        private static Fraction[,] convertToAvailable(ref Fraction[,] matr1, string[] znaki, ref Fraction[] vilni, ref int infinity)
        {

            int size = 0;
            if (znaki[0] == ">=")
            {
                size = matr1.GetLength(0) + 1;
            }
            else
                size = matr1.GetLength(0);
            Fraction[,] m = new Fraction[matr1.GetLength(0), matr1.GetLength(1) + size];
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    m[i, j] = new Fraction(0);
                }
            }
            for (int i = 0; i < matr1.GetLength(0); i++)
            {
                for (int j = 0; j < matr1.GetLength(1); j++)
                {
                    m[i, j] = matr1[i, j];
                }
            }
            int max = 0;
            Fraction maxValue = new Fraction(double.MinValue);
            if (znaki[0] == ">=")
            {
                for (int i = 0; i < m.GetLength(0); i++)
                {
                    for (int j = 0; j < m.GetLength(1); j++)
                    {
                        m[i, j + i] = new Fraction(-1);
                    }
                }


                for (int i = 0; i < vilni.Length; i++)
                {
                    if (vilni[i] > maxValue)
                    {
                        max = i;
                        maxValue = vilni[i];
                    }
                }
                for (int i = 0; i < m.GetLength(0); i++)
                {
                    if (i != max)
                    {
                        for (int j = 0; j < m.GetLength(1); j++)
                        {
                            m[i, j] = m[max, j] - m[i, j];
                        }
                    }
                }
                m[max, m.GetLength(1) - 1] = new Fraction(1);
                infinity = max;
            }
            else
                for (int i = 0; i < m.GetLength(0); i++)
                {
                    m[i, m.GetLength(0) + i] = new Fraction(1);
                }
            return m;
        }


        private static void createNewBasisForward(ref Fraction[,] matr, ref Fraction[] vilni, ref Fraction sum, ref Fraction[] ocinki, ref int[] basis, Fraction[] Fx)
        {
            Fraction maxValue = new Fraction(0);
            int max = 0;
            for (int i = 0; i < ocinki.Length; i++)
            {
                if (ocinki[i].sign == 1)
                {
                    if (ocinki[i] > maxValue)
                    {
                        maxValue = ocinki[i];
                        max = i;
                    }
                }
            }

            Fraction minValue = new Fraction(Double.MaxValue);
            int min = 0;
            for (int i = 0; i < vilni.Length; i++)
            {
                Fraction res = vilni[i] / matr[i, max];
                if (res.sign == 1 && res < minValue)
                {
                    min = i;
                    minValue = res;
                }
            }
            basis[min] = max;
            Fraction key = matr[min, max];
            Fraction key2 = ocinki[max];
            key2.sign = key2.sign * (-1);
            for (int i = 0; i < matr.GetLength(1); i++)
                matr[min, i] /= key;
            vilni[min] /= key;
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                if (i != min)
                {
                    key = matr[i, max];
                    if (key.sign == 1)
                        key.sign = -1;
                    else
                        key.sign = 1;
                    vilni[i] = vilni[i] + (vilni[min] * key);
                    for (int j = 0; j < matr.GetLength(1); j++)
                    {
                        if (i != min)
                        {
                            if (j == max)
                                matr[i, j] = new Fraction(0);
                            else
                                matr[i, j] = matr[i, j] + (matr[min, j] * key);
                        }
                    }
                }

            }
            for (int i = 0; i < ocinki.Length; i++)
            {
                if (i == max)
                    ocinki[i] = new Fraction(0);
                else
                    ocinki[i] = ocinki[i] + (matr[min, i] * key2);
            }
            sum = new Fraction(0);
            for (int i = 0; i < basis.Length; i++)
            {
                sum = sum + (vilni[i] * Fx[basis[i]]);
            }



        }

        private static int calculate(Fraction[,] matr, Fraction[] ocinki, Fraction[] vilni)
        {
            //int res = 0;
            //int count = 0, count2=0;
            //bool isAvailable = false;
            //for (int i = 0; i < vilni.Length; i++)
            //{
            //    if (vilni[i].sign == 1)
            //    {
            //        count++;
            //        if (count == vilni.Length )
            //        {
            //            return res=1;
            //        }
            //    }
            //    else
            //    {
            //        count2 = 0;
            //       for (int j = 0; j < matr.GetLength(1); j++)
            //        {
            //            if (matr[i, j].sign == 1)
            //            {
            //                count2++;
            //                if (count2 == matr.GetLength(1))
            //                    return res=3;
            //            }

            //        }

            //    }
            //}
            //return res = 2;
            int res = 0;
            int count = 0, count2 = 0;
            for (int i = 0; i < ocinki.Length; i++)
            {
                if (ocinki[i].sign == -1 || ocinki[i].numerator == 0.0)
                {
                    count++;
                    if (count == ocinki.Length )
                    {
                        res = 1;
                        return res;
                    }
                }
                else
                {
                    for (int j = 0; j < matr.GetLength(0); j++)
                    {
                        if (matr[j, i] > 0)
                        {
                            res = 2;
                            return res;
                        }
                        else
                        {
                            count2++;
                            if (count2 == matr.GetLength(0))
                            {
                                res = 3;
                                return res;
                            }
                        }

                    }
                }
            }
            return res;
        }

        private static void showTable(Fraction[,] matr, Fraction[] vilni, int[] basis, Fraction sum, Fraction[] ocinki)
        {
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                Console.Write("X" + (basis[i] + 1) + " ");
                for (int k = 0; k < matr.GetLength(1); k++)
                {
                    if (matr[i, k].sign == -1)
                        Console.Write("-");

                    matr[i, k].Reduce();
                    if (matr[i, k].denominator == 1.0)
                        Console.Write(matr[i, k].numerator + "   ");
                    else
                        if (matr[i, k].numerator == matr[i, k].denominator)
                        Console.Write(1);
                    else
                        Console.Write(matr[i, k].numerator + "/" + matr[i, k].denominator + "   ");
                }
                vilni[i].Reduce();
                if (vilni[i].sign == -1)
                    Console.Write("-");
                if (vilni[i].denominator == 1.0)
                    Console.Write(vilni[i].numerator);
                else
                    if (vilni[i].numerator == vilni[i].denominator)
                    Console.Write(1);
                else
                    Console.Write(vilni[i].numerator + "/" + vilni[i].denominator);
                Console.WriteLine();
            }
            Console.Write("   ");
            for (int i = 0; i < matr.GetLength(1); i++)
            {
                if (ocinki[i].sign == -1)
                    Console.Write("-");
                ocinki[i].Reduce();
                if (ocinki[i].denominator == 1.0)
                    Console.Write(ocinki[i].numerator + "   ");
                else
                    if (ocinki[i].denominator == ocinki[i].numerator)
                    Console.Write(1);
                else
                    Console.Write(ocinki[i].numerator + "/" + ocinki[i].denominator + "   ");
            }
            if (sum.sign == -1)
                Console.Write("-");
            sum.Reduce();
            if (sum.denominator == 1.0)
                Console.Write(sum.numerator);
            else
                if (sum.denominator == sum.numerator)
                Console.Write(1);
            else
                Console.Write(sum.numerator + "/" + sum.denominator);
            Console.WriteLine();
            Console.WriteLine("____________________________________");
        }
    }

    public class Fraction
    {
        public double numerator;
        public double denominator;
        public int sign;

        public Fraction(double numerator, double denominator)
        {
            this.numerator = Math.Abs(numerator);
            this.denominator = Math.Abs(denominator);

            if (numerator * denominator < 0)
            {
                this.sign = -1;
            }
            else
            {
                this.sign = 1;
            }
        }

        public Fraction(double numerator)
        {
            this.numerator = Math.Abs(numerator);
            this.denominator = 1.0;

            if (numerator * denominator < 0)
            {
                this.sign = -1;
            }
            else
            {
                this.sign = 1;
            }
        }

        public Fraction(double numerator, double denominator, int v)
        {
            this.numerator = Math.Abs(numerator);
            this.denominator = Math.Abs(denominator);
            this.sign = -1;
        }

        public static bool operator >(Fraction a, Fraction b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator >(Fraction a, double b)
        {
            return a > new Fraction(b);
        }

        public static bool operator <(Fraction a, double b)
        {
            return a < new Fraction(b);
        }
        public static bool operator <(Fraction a, Fraction b)
        {
            return a.CompareTo(b) < 0;
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            return a * b.GetReverse();
        }

        private int CompareTo(Fraction that)
        {
            if (this.Equals(that))
            {
                return 0;
            }
            Fraction a = this.Reduce();
            Fraction b = that.Reduce();
            if (a.numerator * a.sign * b.denominator > b.numerator * b.sign * a.denominator)
            {
                return 1;
            }
            return -1;
        }

        public static Fraction operator *(Fraction a, double b)
        {
            return a * new Fraction(b);
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction(a.numerator * a.sign * b.numerator * b.sign, a.denominator * b.denominator);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            return performOperation(a, b, (double x, double y) => x - y);
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            return performOperation(a, b, (double x, double y) => x + y);
        }
        private static Fraction performOperation(Fraction a, Fraction b, Func<double, double, double> operation)
        {
            double leastCommonMultiple = getLeastCommonMultiple(a.denominator, b.denominator);

            double additionalMultiplierFirst = leastCommonMultiple / a.denominator;

            double additionalMultiplierSecond = leastCommonMultiple / b.denominator;

            double operationResult = operation(a.numerator * additionalMultiplierFirst * a.sign,
                                            b.numerator * additionalMultiplierSecond * b.sign);

            return new Fraction(operationResult, a.denominator * additionalMultiplierFirst);
        }

        private Fraction GetReverse()
        {
            return new Fraction(this.denominator * this.sign, this.numerator);
        }

        public Fraction Reduce()
        {
            Fraction result = this;
            double greatestCommonDivisor = getGreatestCommonDivisor(this.numerator, this.denominator);
            result.numerator /= greatestCommonDivisor;
            result.denominator /= greatestCommonDivisor;
            return result;
        }

        private static double getLeastCommonMultiple(double a, double b)
        {
            return a * b / getGreatestCommonDivisor(a, b);
        }

        private static double getGreatestCommonDivisor(double a, double b)
        {
            while (b != 0)
            {
                double temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}
