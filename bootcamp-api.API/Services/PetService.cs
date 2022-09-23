using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Domain;
using bootcamp_api.Data;
using bootcamp_api.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace bootcamp_api.Services
{
    public class PetService: IPetService
    {

        private readonly PawssierContext _context;
        private readonly IMapper _mapper;

        public PetService(PawssierContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Dto.PetListItem[] GetAll(ApiVersion version, int user_id)
        {
            var pets = _context.Pets
                .Include(p => p.Conditions)
                .Include(p => p.Vaccines)
                .Include(p => p.Prescriptions)
                .Include(p => p.PetPhoto)
                .Include(p => p.VetRecords)
                .Where(p => p.User_id == user_id);

            var petAry = pets.OrderBy(p => p.Id).ToArray();

            var petListItems = _mapper.Map<Pet[], Dto.PetListItem[]>(petAry);

            foreach(Dto.PetListItem x in petListItems)
            {
                x.Link = "/api/v" + version + "/Pets/" + x.Id;
            }

            return petListItems;
        }

        public Dto.Pet Get(int id)
        {
            var pet = _context.Pets
                .Include(p => p.Conditions)
                .Include(p => p.Vaccines)
                .Include(p => p.Prescriptions)
                .Include(p => p.PetPhoto)
                .Include(p => p.VetRecords)
                .SingleOrDefault(p => p.Id == id);

            if (pet == null)
                throw new PetNotFoundException(id);

            return _mapper.Map<Pet, Dto.Pet>(pet);
        }

        public Dto.Pet Add(int user_id, Dto.Pet pet)
        {
            DateTime now = DateTime.Now;

            var newPet = new Pet
            {
                AdoptionDay = pet.AdoptionDay,
                Breed = pet.Breed,
                Birthday = pet.Birthday,
                Color = pet.Color,
                Description = pet.Description,
                Fixed = pet.Fixed,
                Microchip = pet.Microchip,
                Name = pet.Name,
                Sex = pet.Sex,
                Weight = pet.Weight,
                DateAdded = now,
                DateModified = now,
                User_id = user_id
            };

            if(pet.Conditions is not null)
                foreach(Dto.Condition c in pet.Conditions)
                {
                    newPet.Conditions.Add(new Condition
                    {
                        Name = c.Name,
                        Notes = c.Notes
                    });
                }

            if(pet.Prescriptions is not null)
                foreach (Dto.Prescription p in pet.Prescriptions)
                {
                    newPet.Prescriptions.Add(new Prescription
                    {
                        Name = p.Name,
                        Doctor = p.Doctor,
                        Due = p.Due,
                        Refills = p.Refills
                    });
                }

            if(pet.Vaccines is not null)
                foreach (Dto.Vaccine p in pet.Vaccines)
                {
                    newPet.Vaccines.Add(new Vaccine
                    {
                        Name = p.Name,
                        DateAdministered = p.DateAdministered,
                        DueDate = p.DueDate
                    });
                }

            if(pet.PetPhoto is not null && pet.PetPhoto.DbPath is not null)
                newPet.PetPhoto = new FileLink
                {
                    DbPath = pet.PetPhoto.DbPath
                };

            if (pet.VetRecords is not null && pet.VetRecords.DbPath is not null)
                newPet.VetRecords = new FileLink
                {
                    DbPath = pet.VetRecords.DbPath
                };

            _context.Pets.Add(newPet);
            _context.SaveChanges();

            return _mapper.Map<Pet, Dto.Pet>(newPet);
        }

        public void Delete(int id)
        {
            var pet = _context.Pets
                .Include(p => p.Conditions)
                .Include(p => p.Prescriptions)
                .Include(p => p.Vaccines)
                .Include(p => p.PetPhoto)
                .Include(p => p.VetRecords)
                .SingleOrDefault(p => p.Id == id)
;
            if (pet is null)
                throw new PetNotFoundException(id);

            _context.Conditions.RemoveRange(pet.Conditions);
            _context.Vaccines.RemoveRange(pet.Vaccines);
            _context.Prescriptions.RemoveRange(pet.Prescriptions);
            if(pet.PetPhoto is not null)
                _context.FileLinks.Remove(pet.PetPhoto);
            if(pet.VetRecords is not null)
                _context.FileLinks.Remove(pet.VetRecords);

            _context.Remove(pet);
            _context.SaveChanges();
        }

        public Dto.Pet Update(int id, Dto.Pet pet)
        {
            if (id != pet.Id)
                throw new Exception();

            var existingPet = _context.Pets
                .Include(p => p.Conditions)
                .Include(p => p.Prescriptions)
                .Include(p => p.Vaccines)
                .Include(p => p.PetPhoto)
                .Include(p => p.VetRecords)
                .SingleOrDefault(p => p.Id == id);

            if (existingPet == null)
                throw new PetNotFoundException(id);

            existingPet.AdoptionDay = pet.AdoptionDay;
            existingPet.Breed = pet.Breed;
            existingPet.Birthday = pet.Birthday;
            existingPet.Color = pet.Color;
            existingPet.Description = pet.Description;
            existingPet.Fixed = pet.Fixed;
            existingPet.Microchip = pet.Microchip;
            existingPet.Name = pet.Name;
            existingPet.Sex = pet.Sex;
            existingPet.Weight = pet.Weight;
            existingPet.DateModified = DateTime.Now;

            _context.Conditions.RemoveRange(existingPet.Conditions);
            _context.Vaccines.RemoveRange(existingPet.Vaccines);
            _context.Prescriptions.RemoveRange(existingPet.Prescriptions);

            foreach (Dto.Condition c in pet.Conditions)
            {
                existingPet.Conditions.Add(new Condition
                {
                    Name = c.Name,
                    Notes = c.Notes
                });
            }

            foreach (Dto.Prescription p in pet.Prescriptions)
            {
                existingPet.Prescriptions.Add(new Prescription
                {
                    Name = p.Name,
                    Doctor = p.Doctor,
                    Due = p.Due,
                    Refills = p.Refills
                });
            }

            foreach (Dto.Vaccine p in pet.Vaccines)
            {
                existingPet.Vaccines.Add(new Vaccine
                {
                    Name = p.Name,
                    DateAdministered = p.DateAdministered,
                    DueDate = p.DueDate
                });
            }

            var oldPhoto = existingPet.PetPhoto;
            var oldRecords = existingPet.VetRecords;

            if (pet.PetPhoto is not null && pet.PetPhoto.DbPath is not null)
                existingPet.PetPhoto = new FileLink
                {
                    DbPath = pet.PetPhoto.DbPath
                };
            else
            {
                existingPet.PetPhoto = null;
            }

            if (pet.VetRecords is not null && pet.VetRecords.DbPath is not null)
                existingPet.VetRecords = new FileLink
                {
                    DbPath = pet.VetRecords.DbPath
                };
            else
            {
                existingPet.VetRecords = null;
            }

            if(oldPhoto is not null)
                _context.FileLinks.Remove(oldPhoto);
            if(oldRecords is not null)
                _context.FileLinks.Remove(oldRecords);

            _context.SaveChanges();

            return _mapper.Map<Pet, Dto.Pet>(existingPet);
        }
    }
}