﻿using System.Runtime.InteropServices;
using BankingSystem.DBContext;
using BankingSystem.ViewModels;

namespace BankingSystem.DbOperations;

public interface IOrganizationRepository
{
    Task<OrganizationViewModel> GetOrganization(string modelRegNo);
    Task<List<OrganizationIndividualViewModel>?> GetOrganizationIndividuals(string modelRegNo);
}

public class OrganizationRepository : IOrganizationRepository
{
    private readonly AppDbContext _context;

    public OrganizationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrganizationViewModel> GetOrganization(string modelRegNo)
    {
        var conn = _context.GetConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT * FROM organization WHERE reg_no = @regNo";
        cmd.Parameters.AddWithValue("@regNo", modelRegNo);
        using var reader = await cmd.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
            return null;
        }

        var organization = new OrganizationViewModel();
        await reader.ReadAsync();
        organization.CustomerId = reader.GetInt32("customer_id");
        organization.RegNo = reader.GetString("reg_no");
        organization.Name = reader.GetString("name");
        organization.Address = reader.GetString("address");
        organization.CompanyEmail = reader.GetString("company_email");
        organization.Type = reader.GetString("type");
        await conn.CloseAsync();
        return organization;
    }

    public async Task<List<OrganizationIndividualViewModel>?> GetOrganizationIndividuals(string modelRegNo)
    {
        var conn = _context.GetConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT * FROM organization_individual JOIN individual 
             ON organization_individual.individual_id = individual.individual_id;";
        cmd.Parameters.AddWithValue("@regNo", modelRegNo);
        using var reader = await cmd.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
            return null;
        }
        
        List<OrganizationIndividualViewModel> individuals = new();

        while (await reader.ReadAsync())
        {
            var individual = new OrganizationIndividualViewModel();
            individual.CustomerId = reader.GetInt32("customer_id");
            individual.NIC = reader.GetString("NIC");
            individual.Name = $"{reader.GetString("first_name")} {reader.GetString("last_name")}";
            individual.WorkEmail = reader.GetString("work_email");
            individual.Position = reader.GetString("position");
            individuals.Add(individual);
        }
        
        await conn.CloseAsync();

        return individuals;
    }
}