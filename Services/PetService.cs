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

namespace bootcamp_api.Services
{
    public class PetService: IPetService
    {

        private readonly PawssierContext _context;

        public PetService(PawssierContext context)
        {
            _context = context;
        }

        public Pet[] GetAll()
        {
            var pets = _context.Pets;

            return pets.OrderBy(p => p.Id).ToArray();
        }

        public Pet Get(int id)
        {
            var pet = _context.Pets.SingleOrDefault(p => p.Id == id);
            if (pet == null)
                throw new PetNotFoundException(id);

            return pet;
        }

        public Pet Add(Dto.Pet pet)
        {
            DateTime now = DateTime.Now;

            Condition[] conditions = new Condition[0];
            if(pet.Conditions is not null)
            {
                foreach(Dto.Condition c in pet.Conditions)
                {
                    var newCondition = new Condition
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Notes = c.Notes is not null ? c.Notes : ""
                    };
                    conditions.Append(newCondition);
                }
            }
            Prescription[] prescriptions = new Prescription[0];
            if (pet.Prescriptions is not null)
            {
                foreach (Dto.Prescription p in pet.Prescriptions)
                {
                    var newPrescription = new Prescription
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Doctor = p.Doctor,
                        Due = p.Due,
                        Refills = p.Refills
                    };
                    prescriptions.Append(newPrescription);
                }
            }
            Vaccine[] vaccines = new Vaccine[0];
            if (pet.Vaccines is not null)
            {
                foreach (Dto.Vaccine p in pet.Vaccines)
                {
                    var newVaccine = new Vaccine
                    {
                        Id = p.Id,
                        Name = p.Name,
                        DateAdministered = p.DateAdministered,
                        DueDate = p.DueDate
                    };
                    vaccines.Append(newVaccine);
                }
            }
            FileLink petPhoto = new FileLink
            {
                DbPath = pet.PetPhoto is not null ? pet.PetPhoto.DbPath : ""
            };
            FileLink vetRecords = new FileLink
            {
                DbPath = pet.VetRecords is not null ? pet.VetRecords.DbPath : ""
            };
            var newPet = new Pet
            {
                Id = pet.Id,
                AdoptionDay = pet.AdoptionDay,
                Breed = pet.Breed,
                Birthday = pet.Birthday,
                Color = pet.Color,
                Conditions = conditions,
                Description = pet.Description,
                Fixed = pet.Fixed,
                Microchip = pet.Microchip,
                Name = pet.Name,
                PetPhoto = pet.PetPhoto is null ? petPhoto : null,
                Prescriptions = prescriptions,
                Sex = pet.Sex,
                Vaccines = vaccines,
                VetRecords = pet.VetRecords is null ? vetRecords : null,
                Weight = pet.Weight
            };

            _context.Pets.Add(newPet);
            _context.SaveChanges();

            return newPet;
        }

        public void Delete(int id)
        {
            var pet = _context.Pets.SingleOrDefault(p => p.Id == id);
            if (pet is null)
                throw new PetNotFoundException(id);

            _context.Remove(pet);
            _context.SaveChanges();
        }

        public Pet Update(int id, Dto.Pet pet)
        {
            var existingPet = _context.Pets.SingleOrDefault(p => p.Id == id);
            if (existingPet == null)
                throw new PetNotFoundException(id);
            /*
            existingPet.DateModified = DateTime.Now;
            existingPet.AdoptionDay = pet.AdoptionDay;
            existingPet.Breed = pet.Breed;
            existingPet.Birthday = pet.Birthday;
            existingPet.Color = pet.Color;
            existingPet.Conditions = pet.Conditions;
            existingPet.Description = pet.Description;
            existingPet.Fixed = pet.Fixed;
            existingPet.Microchip = pet.Microchip;
            existingPet.Name = pet.Name;
            existingPet.PetPhoto = pet.PetPhoto;
            existingPet.Prescriptions = pet.Prescriptions;
            existingPet.Sex = pet.Sex;
            existingPet.Vaccines = pet.Vaccines;
            existingPet.VetRecords = pet.VetRecords;
            existingPet.Weight = pet.Weight;
            
            _context.SaveChanges();
*/
            return existingPet;
        }

        /*private static mapConditions(Condition[] c)
        {
            if(c is null)
            {
                return null;
            }
            Condition[] conditions = new Condition[c];
            foreach(Dto.Condition c in pet.Conditions)
            {
                var newCondition = new Condition
                {
                    Id = c.Id,
                    Name = c.Name,
                    Notes = c.Notes is not null ? c.Notes : "",
                    DateAdded = now,
                    DateModified = now
                };
                conditions.Append(newCondition);
            }
        } */
    }
}