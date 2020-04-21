namespace IntroSE.Kanban.Backend.ServiceLayer
{
    ///<summary>This class extends <c>Response</c> and represents the result of a call to a non-void function. 
    ///In addition to the behavior of <c>Response</c>, the class holds the value of the returned value in the variable <c>Value</c>.</summary>
    ///<typeparam name="T">The type of the returned value of the function, stored by the list.</typeparam>
    public class Response<T> : Response
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public readonly T Value;
        internal Response(string msg) : base(msg) { }
        internal Response(T value) : base()
        {           
            log.Debug("Response<"+Value.GetType()+"> was created.");
            this.Value = value;
        }
        internal Response(T value, string msg) : base(msg)
        {
            log.Debug("Response<" + Value.GetType() + "> was created.");
            this.Value = value;
        }
    }
}
