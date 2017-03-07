<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurveyMOU.ascx.cs" Inherits="sselOnLine.Controls.SurveyMOU" %>
<asp:Panel runat="server" ID="panMain" Visible="false">
    <div class="survey">
        <div class="survey-title">
            University of Michigan Lurie Nanofabrication Facility Memorandum of Understanding and Release of Liability
        </div>
        <div class="survey-text" style="white-space: pre-line;">
            I wish to utilize the Lurie Nanofabrication Laboratory (LNF) both during regular working hours when the laboratory is staffed by University employees and after hours when the laboratory is not (or minimally) staffed by University employees.

I realize that the LNF is a shared resource and that I am part of a community of users that agree to a set of guidelines, rules, and disciplinary measures to provide for the common good, mutual safety, and enhanced productivity of all the users in this community.  Violations 
of the principle of “the common good” for the benefit of an individual will not be allowed.  Violations of this principle could result in the loss of my privileges to use the LNF on a temporary or a permanent basis.

I also realize that the tooling contained within the LNF is designed for a specific purpose (or operating regime) and may not meet the needs of my research, my work product, or my goals.  Every effort to meet the needs of any individual user will be balanced against the needs of the user community for whom the tool was originally designed.  This also applies to how the tool is currently operated.  Alterations to the tooling design, process, or current operational mode will be at the discretion of the LNF Managing Director.  If suggested alterations to tooling, processes, or current operating modes within the LNF are to be undertaken (as requested by an individual user or user group), the LNF Managing Director will take into consideration the needs of the community to assure that the common good of the user-base is always taken into consideration prior to any modifications.  Any process development or tool redesign beyond the current tool’s established operating parameters will be subject to additional fees.

I further realize that items sold in the LNF store have no actual or implied warranty, guarantee, or LNF-supported technical specification and are provided as a convenience to the user who assumes all responsibility for using LNF store items.
 
I recognize that my research, work product, or study in the LNF may entail the handling and disposing of a number of hazardous materials such as solvents, oxidizers, and acids, which may cause severe injury or even death if not handled and disposed of properly.  In some cases flammable and toxic gases, such as hydrogen, arsine, and phosphine, are also used and, if not handled properly, may also result in injury or death to myself or others. 

I also recognize that there exists equipment in the LNF which may cause serious injury or death if not operated or otherwise handled according to established written documentation, historical precedent, currently accepted practice, or verbal instruction by University personnel.

I further recognize that potential hazards such as shock, ultraviolet (UV) exposure, and non-ionizing radiation all exist within the LNF.  There could also exist non-documented non-obvious hazards that may not be known to the, University, its staff or other users within the LNF. These hazards could harm or result in death to personnel (yourself, another user, staff, visitors) within the facility, building, or area if an engineering or administrative control were to fail or not exist. Therefore, I agree to always maintain a vigilant awareness of the environment I am entering and take responsibility for my actions within that environment at all times.
 
I acknowledge that the LNF uses video cameras to monitor several areas within the facility and that LNF staff can review recorded images to maintain safety, security and adherence to LNF policies and protocols.

I also acknowledge that I have satisfied all safety requirements which includes, but is not limited to:  lab safety training and testing, personal communications, and any required orientation.

I further acknowledge that the U-M OSEH lab safety training, LNF safety manuals, and all applicable standard operating procedures (SOPs) or best known methods (BKMs) have been clearly explained and that the dangers and proper procedures for handling the various materials and equipment in the laboratory safely have been described to my satisfaction. I understand the risks and dangers resulting from not following such standard operating procedures (SOPs) or best known methods (BKMs) . I also realize that not every possible use or outcome of working with hazardous materials within the LNF has been reviewed by University personnel and that common sense and accepted practices must always be paramount when utilizing this facility.

I further acknowledge that using the the LNF equipment for purposes other than specifically what it was designed for is not allowed or recommended.

I further acknowledge that it is my responsibility to assure that I have discussed this document in full with my advisor, mentor, company, non-UM university, or school, and to provide them with a copy of the signed document.

I agree to wear a pair of safety glasses or chemical splash goggles, which conform to the ANSI Z87.1-2010 standard, at all times when inside the LNF areas and realize that street glasses are not sufficient and must be used in conjunction with the proper safety eyewear.  In addition, contact lenses are not banned from the LNF but it is highly recommended that they not be used within the LNF due to possible exposure and/or entrapment of chemicals behind or within the contacts that could damage your eyesight or even cause blindness.

I also agree to always wear the proper personal protective equipment PPE) as prescribed by the documentation of the LNF, direction of the staff or as dictated by accepted practice or common sense. 

I further agree that working alone in the LNF is not recommended, and is forbidden when using chemicals: I should always utilize the buddy system when working with chemicals or at a chemical bench.

I further agree to the LNF disciplinary policy for violation of this agreement, which can result in denial of access to the LNF outside of normal business hours (M - F 8 am to 5 pm) or permanent loss of access privileges from the LNF. The ultimate interpretation of any given incident or series of incidents will be at the discretion of the LNF Managing Director.  Incidents will be tracked using the LNF Feedback System: for any incident or series of incidents to be considered a violation they must be recorded in the LNF Feedback System.

I further agree to abide by any measures to assure that the facility remains secure.  This includes reporting suspicious activity, not leaving doors open or unlocked, and not going into areas that are not intended for users (such as the subfab, basement, or service aisles unless accompanied by the proper University personnel).

I further agree to report any violations of the rules, guidelines, or verbal instructions utilizing the LNF Feedback System.

In consideration of the opportunity to use the LNF I agree to meet my financial obligations to the facility which include laboratory usage charges and damage to equipment if that damage is deemed to be as a result of my failure to follow the guidelines, rules, verbal communications, SOPs, BKMs, common sense, or malicious intent.Additionally, it is understood that LNF-controlled items (such as gloves, gowns, chemicals, tools, instruments, computers, or lab supplies) within the LNF are considered the property of the University and shall not be removed from the LNF.  Unauthorized removal of material from the LNF will constitute theft and will be reported to the UMPD.In addition, my privileges regarding use of the LNF will be revoked permanently.
 
In further consideration of the opportunity to use the LNF, I, on behalf of myself, my agents, heirs and personal representatives, agree to release the University and its agents and employees from all responsibility or liability for personal injury, including death, and damage to or loss of personal property, which I may incur due to my failure to follow any safety precautions, procedures, directions, and/or instructions communicated to me and/or due to my own negligence in handling of operation materials and/or equipment in the laboratory. This release does not apply to personal injury, including death, or property damage or loss caused by the negligence or intentional misconduct of the University and its employees and agents. 

I, the undersigned, am at least eighteen (18) years of age, am competent to sign this release, and have read carefully and understood all of its terms.
        </div>
        <div class="survey-text">
            <asp:Button ID="btnAgree" runat="server" Text="Agree" OnClick="btnAgree_Click" />
        </div>
    </div>
</asp:Panel>
