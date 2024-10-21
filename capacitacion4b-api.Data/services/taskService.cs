using capacitacion4b_api.Data.interfaces;
using capacitacion4b_api.DTOs.task;
using capacitacion4b_api.DTOs.user;
using capacitacion4b_api.Models;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capacitacion4b_api.Data.services { 

  public class taskService : iTaskService    
   {

 private postgresqlConection _postgresqlConection;
        public taskService(postgresqlConection postgresqlConection) => _postgresqlConection = postgresqlConection;

        private NpgsqlConnection GetConnection() => new NpgsqlConnection(_postgresqlConection._connection);

       
        #region fnidalltask
        public async Task<IEnumerable<taskModel>> FindAll()
        {

            string sqlQuery = "select * from view_tarea";

            using NpgsqlConnection database = GetConnection();

            try
            {

                /* abre conexión */
                await database.OpenAsync();

                /* ejecuta el query */
                IEnumerable<taskModel> allTask = await database.QueryAsync<taskModel>(sqlQuery);

                /* cierra conexión*/
                await database.CloseAsync();

                return allTask;

            }
            catch (Exception e)
            {
                return null;
            }

        }

        #endregion

        #region findonetask
        public async Task<taskModel> FindOne(int idTarea)
        {
            string sqlQuery = "select * from view_tarea where idTarea = @idTarea";
            using NpgsqlConnection database = GetConnection();

            try
            {

                /* abre conexión */
                await database.OpenAsync();

                /* ejecuta el query */
                taskModel? task = await database.QueryFirstOrDefaultAsync<taskModel>(sqlQuery, new { idTarea });

                /* cierra conexión*/
                await database.CloseAsync();

                return task;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        #endregion
        #region taskCreate
        public async Task<taskModel?> Create(createTaskDto createTaskDto)
        {

            string sqlQuery = "select * from fun_task_create (" +
                "p_tarea := @tarea," +
                "p_description := @description," +
                "p_fk_idUsuario := @idUsuario);";

            using NpgsqlConnection database = GetConnection();

            try
            {

                /* abre conexión */
                await database.OpenAsync();

                /* ejecuta el query */
                taskModel? task = await database.QueryFirstOrDefaultAsync<taskModel>(sqlQuery, param: new
                {

                    tarea = createTaskDto.tarea,
                    description = createTaskDto.description,
                    idUsuario = createTaskDto.idUsuario

                });

                /* cierra conexión*/
                await database.CloseAsync();

                return task;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        #endregion
        #region updaretask
        public async Task<taskModel?> Update(int idTarea, updateTaskDto updateTaskDto)
        {
            /* query de function */
            string sqlQuery = "select * from fun_tarea_updat (" +
                "p_idTarea := @idTarea," +
                "p_tarea := @tarea," +
                "p_description := @description);";

            /* usamos la conexión */
            using NpgsqlConnection database = GetConnection();

            try
            {

                /* abre conexión */
                await database.OpenAsync();

                /* ejecuta el query */
                taskModel? task = await database.QueryFirstOrDefaultAsync<taskModel>(sqlQuery,
                param: new
                {

                    /* parametros */
                    idTarea = idTarea,
                    tarea = updateTaskDto.tarea,
                    description = updateTaskDto.description

                }
                );

                /* cierra conexión*/
                await database.CloseAsync();

                return task;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        #endregion
        #region fundelete
        public async Task<taskModel> Remove(int idTarea)
        {
            string sqlQuery = "select * from fun_task_remove ( p_idTarea := @idTarea );";

            using NpgsqlConnection database = GetConnection();

            try
            {

                /* abre conexión */
                await database.OpenAsync();

                /* ejecuta el query */
                taskModel? task = await database.QueryFirstOrDefaultAsync<taskModel>(sqlQuery, param: new
                {

                    idTarea

                });

                /* cierra conexión*/
                await database.CloseAsync();

                return task;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        #endregion

        #region togglestatus
        public async Task<taskModel> ToggleStatus(int idTarea)
        {

            string sqlQuery = "select * from fun_task_togglestatus ( p_idTarea := @idTarea );";

            using NpgsqlConnection database = GetConnection();

            try
            {

                /* abre conexión */
                await database.OpenAsync();

                /* ejecuta el query */
                taskModel? task = await database.QueryFirstOrDefaultAsync<taskModel>(sqlQuery, param: new
                {

                    idTarea

                });

                /* cierra conexión*/
                await database.CloseAsync();

                return task;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        #endregion
    }
}
