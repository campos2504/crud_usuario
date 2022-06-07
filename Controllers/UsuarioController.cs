using Microsoft.AspNetCore.Mvc;
using usuario.Model;
using usuario.Repository;

namespace usuario.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioController(IUsuarioRepository repository)
        {
            _repository = repository;
        }



        private static List<Usuario> Usuarios()
        {
            return new List<Usuario>{
                new Usuario{Nome="Lucas", Id=1, DataNascimento=new DateTime(1994,05,01)}
            };

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var usuarios = await _repository.BuscaUsuarios();
            return usuarios.Any() ?
             Ok(usuarios) :
             NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Usuario usuario)
        {

            _repository.AdicionaUsuario(usuario);
            return await _repository.SaveChangesAsync() ?
            Ok("Usuário adicionado com sucesso") :
            BadRequest("Erro ao salvar o usuário");

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            var usuarios = await _repository.BuscaUsuario(id);
            return usuarios != null ?
             Ok(usuarios) :
             NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Usuario usuario)
        {
            var usuarioBanco = await _repository.BuscaUsuario(id);
            if (usuarioBanco == null) return NotFound("Usuário não encontrado");

            usuarioBanco.Nome = usuario.Nome ?? usuarioBanco.Nome;
            usuarioBanco.DataNascimento = usuario.DataNascimento != new DateTime() ?
            usuario.DataNascimento : usuarioBanco.DataNascimento;

            _repository.AtualizarUsuario(usuarioBanco);
            return await _repository.SaveChangesAsync() ?
            Ok("Usuário atualizado com sucesso") :
            BadRequest("Erro ao atualizar o usuário");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioBanco = await _repository.BuscaUsuario(id);
            if (usuarioBanco == null) return NotFound("Usuário não encontrado");

            _repository.DeletarUsuario(usuarioBanco);

            return await _repository.SaveChangesAsync() ?
            Ok("Usuário deletado com sucesso") :
            BadRequest("Erro ao deletar o usuário");
        }

    }
}