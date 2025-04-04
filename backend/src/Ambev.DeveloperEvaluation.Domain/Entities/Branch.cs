using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Domain.Validators;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a physical retail branch location where sales transactions occur
/// </summary>
public class Branch : BaseEntity
{
    // Properties
    /// <summary>
    /// Official name of the branch location
    /// </summary>
    public string Name { get;  set; } = string.Empty;

    /// <summary>
    /// Full physical address of the branch
    /// </summary>
    public Address Address { get; set; }  // Value object

    /// <summary>
    /// Primary contact number for the branch
    /// </summary>
    public string PhoneNumber { get;  set; } = string.Empty;

    /// <summary>
    /// Current operational status of the branch
    /// </summary>
    public BranchStatus Status { get;  set; } = BranchStatus.Active;    

    /// <summary>
    /// Update branch phone number
    /// </summary>
    public void UpdatePhoneNumber(string newPhoneNumber)
    {
        if (string.IsNullOrWhiteSpace(newPhoneNumber))
            throw new DomainException("Phone number cannot be empty");

        PhoneNumber = newPhoneNumber;
    }

    /// <summary>
    /// Updates the branch's operational status
    /// </summary>
    public void ChangeStatus(BranchStatus newStatus)
    {
        if (Status == BranchStatus.ClosedPermanently && newStatus != BranchStatus.ClosedPermanently)
            throw new DomainException("Cannot reopen a permanently closed branch");

        Status = newStatus;
    }


    /// <summary>
    /// Performs validation of the branch entity using the BranchValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    /// <remarks>
    /// <listheader>The validation includes checking:</listheader>
    /// <list type="bullet">AddressName</list>
    /// <list type="bullet">Phone number format</list>
    /// <list type="bullet">Status</list>
    /// 
    /// </remarks>
    public ValidationResultDetail Validate()
    {
        var validator = new BranchValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}