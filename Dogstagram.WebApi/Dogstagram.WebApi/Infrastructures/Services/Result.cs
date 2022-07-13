namespace Dogstagram.WebApi.Infrastructures.Services
{
    public class Result
    {
        public bool Successeded { get; private set; }

        public bool Failure => !this.Successeded;

        public string? Error { get; private set; }

        public static implicit operator Result(bool succeeded)
            => new Result { Successeded = succeeded};

        public static implicit operator Result(string error) 
            => new Result
            {
                Successeded = false,
                Error = error
            };
    }
}
