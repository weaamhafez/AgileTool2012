using System.Collections.Generic;
using System;

namespace Engineer.Model
{
    public class DataTableResult<T>
    {
        private Int32 draw;
        private Int64 recordsTotal;

        private Int64 recordsFiltered;

        private List<T> data;

        public int Draw
        {
            get
            {
                return draw;
            }

            set
            {
                draw = value;
            }
        }

        public long RecordsTotal
        {
            get
            {
                return recordsTotal;
            }

            set
            {
                recordsTotal = value;
            }
        }

        public long RecordsFiltered
        {
            get
            {
                return recordsFiltered;
            }

            set
            {
                recordsFiltered = value;
            }
        }

        public List<T> Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }

        public DataTableResult(Int32 draw, Int64 recordsTotal, List<T> data)
        {
            this.draw = draw;
            this.recordsTotal = recordsTotal;
            this.recordsFiltered = recordsTotal;
            this.data = data;
        }
        public DataTableResult() { }
    }
}