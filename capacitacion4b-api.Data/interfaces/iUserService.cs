﻿using capacitacion4b_api.DTOs.user;
using capacitacion4b_api.Models;

namespace capacitacion4b_api.Data.interfaces
{

    public interface iUserService
    {

        /* buscar todos los usuarios */
        public Task<IEnumerable<userModel>> findAll();

        /* buscar un solo usuario*/
        public Task<userModel> findOne(int idUsuario);
        
        /* crear un usuario */
        public Task<userModel> create(createUserDto createUserDto);

        /* actualizar un usuario */
        public Task<userModel> update(int idUsuario, updateUserDto updateUserDto);

        /* eliminar un usuario */
        public Task<userModel> remove(int idUsuario);

        public Task<IEnumerable<userModel>> FindAlltasks(int idUsuario);


    }

}
