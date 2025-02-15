﻿using capacitacion4b_api.Data.interfaces;
using capacitacion4b_api.DTOs.user;
using capacitacion4b_api.Models;
using Dapper;
using Npgsql;

namespace capacitacion4b_api.Data.services
{
    public class userService : iUserService
    {

        private postgresqlConection _postgresqlConection;
        public userService(postgresqlConection postgresqlConection) => _postgresqlConection = postgresqlConection;

        private NpgsqlConnection GetConnection() => new NpgsqlConnection(_postgresqlConection._connection);

        public async Task<userModel> create(createUserDto createUserDto)
        {

            string sqlQuery = "select * from f_createuser(" +
                "p_nombresUsuario := @nombres," +
                "p_usuarioUsuario := @usuario," +
                "p_contrasenaUsuario := @contrasena);";

            using NpgsqlConnection database = GetConnection();

            try
            {

                /* abre conexión */
                await database.OpenAsync();

                /* ejecuta el query */
                userModel? user = await database.QueryFirstOrDefaultAsync<userModel>(sqlQuery, new
                {

                    nombres = createUserDto.nombres,
                    usuario = createUserDto.usuario,
                    contrasena = createUserDto.contrasena

                });

                /* cierra conexión*/
                await database.CloseAsync();

                return user;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<IEnumerable<userModel>> findAll()
        {

            /* cadena query*/
            string sqlQuery = "select * from v_usuarios";
            using NpgsqlConnection database = GetConnection();

            try
            {

                /* abre conexión */
                await database.OpenAsync();

                /* ejecuta el query */
                IEnumerable<userModel> users = await database.QueryAsync<userModel>(sqlQuery);

                /* cierra conexión*/
                await database.CloseAsync();

                return users;
            }
            catch (Exception e)
            {
                return [];
            }

        }

        public async Task<userModel> findOne(int idUsuario)
        {
            string sqlQuery = "select * from v_usuarios where idUsuario = @idUsuario;";
            using NpgsqlConnection database = GetConnection();

            try
            {

                /* abre conexión */
                await database.OpenAsync();

                /* ejecuta el query */
                userModel? user = await database.QueryFirstOrDefaultAsync<userModel>(sqlQuery, new { idUsuario });

                /* cierra conexión*/
                await database.CloseAsync();

                return user;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<userModel> update(int idUsuario, updateUserDto updateUserDto)
        {

            string sqlQuery = $"select * from fun_user_updat (" +
                "p_idUsuario := @idUsuario," +
                "p_nombresUsuario := @nombres," +
                "p_usuarioUsuario := @usuario," +
                "p_contrasenaUsuario := @contrasena);";

            using NpgsqlConnection database = GetConnection();

            try
            {

                /* abre conexión */
                await database.OpenAsync();

                userModel? user = await database.QueryFirstOrDefaultAsync<userModel>(sqlQuery, new
                {

                    idUsuario = updateUserDto.idUsuario,
                    nombres = updateUserDto.nombres,
                    usuario = updateUserDto.usuario,
                    contrasena = updateUserDto.contrasena

                });

                /* cierra conexión */
                await database.CloseAsync();

                return user;

            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<userModel> remove(int idUsuario)
        {
            string sqlQuery = "select * from function fun_user_remove (p_idUsuario := @idUsuario;)";
            using NpgsqlConnection database = GetConnection();

            try
            {

                /* abre conexión */
                await database.OpenAsync();

                /* ejecuta el query */
                userModel? user = await database.QueryFirstOrDefaultAsync<userModel>(sqlQuery, new { idUsuario });

                /* cierra conexión*/
                await database.CloseAsync();

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
      
        }

        public async Task<IEnumerable<userModel?>> FindAlltasks(int idUsuario)
        {
            string sqlQuery = "select * from v_usuarios;";
            Dictionary<int, List<taskModel>> tasks = [];
            using NpgsqlConnection database = GetConnection();
            try
            {
                await database.OpenAsync();
                IEnumerable<userModel> result = await database.QueryAsync<userModel, taskModel, userModel>(
                    sql: sqlQuery,
                    param: new { },
                    map: (user, task) => {

                        List<taskModel> currenttasks = [];
                        tasks.TryGetValue(user.idUsuario, out currenttasks);
                        currenttasks ??= [];
                        if (currenttasks.Count == 0 && task != null)
                        {
                            currenttasks = [task];
                        }else if(currenttasks.Count > 0 && task != null)
                        {
                            currenttasks.Add(task);
                        } 
                        tasks[user.idUsuario] = currenttasks;
                            
                            return user;

                                       },
                    splitOn: "idTarea"
                    );

                await database.CloseAsync();
                result = result.Select(user => {
                    user.tasks = tasks[user.idUsuario]; 
                    return user;
                });
                return result;
            }
            catch (Exception e)
            {
                return [];
            }
        }
    }

}
