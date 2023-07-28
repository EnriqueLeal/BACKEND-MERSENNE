using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    //public UsuarioController(IRepositoryAsync<Usuario> repository, IMapper mapper) : base(repository, mapper)
    [Route("api/[controller]")]
    [ApiController]
    public class MersenneController : ControllerBase
    {
        private readonly QuizDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        //private readonly IJWTService _jwtService;

        public MersenneController(QuizDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mersenne>> GetMersenne(int id/*,Mersenne mersenne*/)
        {
            double Result = 0;
            int TotalInsertado = 0;
            _context.Database.ExecuteSqlRaw("TRUNCATE TABLE Mersenne");
            if (id < 1)
            {
                int Res = 0;
                return Ok(Res);
            }

            for (int x = 1; x <= 51; x++)
            {
                for (int i = 1; i < x; i++)
                {
                    Result = Validar(i);
                }
                if (Result != 0 && TotalInsertado < id)
                {
                    int Retorno = 0;
                    Result = Math.Pow(2, Result);
                    Result = Result - 1;
                    Retorno = Validar2((int)Result);
                    int verdadero = 1;
                    if (Retorno >= 1)
                    {
                        _context.Database.ExecuteSqlRaw("INSERT INTO [Mersenne] ([Cantidad], [Activo]) VALUES (" + Result + ", '" + verdadero + "'  );");
                        TotalInsertado++;
                    }

                    
                }
                else
                {

                }
            }
           
            return Ok(Result);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Mersenne>>> GetAll()
        {
            return await _context.Mersenne.ToListAsync();
        }

        [HttpPost]
        [Route("PostMersenne")]
        public async Task<ActionResult<Mersenne>> PostMersenne(Mersenne mersenne)
        {

            _context.Mersenne.Add(mersenne);
            await _context.SaveChangesAsync();


            return Ok(mersenne);
        }

        private double _Comprobar(int id)
        {
            double Result = 0;
            for (int i = 1; i <= id; i++)
            {
                Result = Validar(i);
            }

            return Result;
            
        }

        private int Validar(int Numero)
        {
            var result = 0;
            int TotalNumeroMarsenne = 20;
            int contador = 0;
            int i = 0;
            Numero += 1;

                for (i = 2; i < Numero; i++)
                {

                    if ((Numero % i) == 0)
                        return Numero = 0;
                }
                contador++;
                
            //}
            
            

            return Numero;
        }

        private int Validar2(int Numero)
        {
            var result = 0;
            int TotalNumeroMarsenne = 20;
            int contador = 0;
            int i = 0;

            for (i = 2; i < Numero; i++)
            {

                if ((Numero % i) == 0)
                    return Numero = 0;
            }
            contador++;

            //}



            return Numero;
        }
    }
}
